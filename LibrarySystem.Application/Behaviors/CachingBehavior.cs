using LibrarySystem.Application.CacheService;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace LibrarySystem.Application.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CachingBehavior<TRequest,TResponse>> _logger;

    public CachingBehavior(
        IDistributedCache cache,
        ILogger<CachingBehavior<TRequest,TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if(request is not ICacheableQuery cacheable)
            return await next();

        var cacheKey = cacheable.CacheKey;

        // Try to read from Redis
        try
        {
            var cachedJson = await _cache.GetStringAsync(cacheKey,cancellationToken);

            if(cachedJson != null)
            {
                _logger.LogInformation("Cache HIT for key {CacheKey}",cacheKey);

                // Determine the inner type of Result<T>
                var responseType = typeof(TResponse);

                if(IsResultType(responseType))
                {
                    var innerType = responseType.GetGenericArguments()[0];
                    var value = JsonSerializer.Deserialize(cachedJson,innerType);

                    return (TResponse)CreateResultSuccess(responseType,value)!;
                }

                // If not Result<T>, just deserialize normally
                return JsonSerializer.Deserialize<TResponse>(cachedJson)!;
            }

            _logger.LogInformation("Cache MISS for key {CacheKey}",cacheKey);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex,"Redis unavailable. Skipping cache read for {CacheKey}",cacheKey);
        }

        // Execute handler
        var response = await next();

        // Try to write to Redis
        try
        {
            string jsonToCache;

            if(IsResultType(typeof(TResponse)))
            {
                var value = GetResultValue(response);
                jsonToCache = JsonSerializer.Serialize(value);
            }
            else
            {
                jsonToCache = JsonSerializer.Serialize(response);
            }

            var options = new DistributedCacheEntryOptions();
            if(cacheable.Expiration.HasValue)
                options.SetAbsoluteExpiration(cacheable.Expiration.Value);

            await _cache.SetStringAsync(cacheKey,jsonToCache,options,cancellationToken);

            _logger.LogInformation("Cache SET for key {CacheKey}",cacheKey);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex,"Redis unavailable. Skipping cache write for {CacheKey}",cacheKey);
        }

        return response;
    }

    private static bool IsResultType(Type type)
        => type.IsGenericType && type.Name.StartsWith("Result");

    private static object? GetResultValue(object result)
    {
        var prop = result.GetType().GetProperty("Value",BindingFlags.Public | BindingFlags.Instance);
        return prop?.GetValue(result);
    }

    private static object? CreateResultSuccess(Type resultType,object? value)
    {
        var method = resultType.GetMethod("Success",BindingFlags.Public | BindingFlags.Static);
        return method?.Invoke(null,new[] { value });
    }
}
