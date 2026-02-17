using LibrarySystem.Application.CacheService;
using LibrarySystem.Domain.Shared;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

public record GetAllBooksCommand() : IRequest<Result<List<GetAllBooksResponse>>>, ICacheableQuery
{
    public string CacheKey => "Books";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(1);

}