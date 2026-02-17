namespace LibrarySystem.Application.CacheService;

public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
