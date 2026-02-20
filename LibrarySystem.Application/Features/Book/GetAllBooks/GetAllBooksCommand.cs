using LibrarySystem.Application.CacheService;
using LibrarySystem.Application.Messaging;
using LibrarySystem.Domain.Shared;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

public sealed record GetAllBooksCommand() : IQuery<List<GetAllBooksResponse>>, ICacheableQuery
{
    public string CacheKey => "Books";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

}