using LibrarySystem.Application.CacheService;
using LibrarySystem.Domain.Shared;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Features.Book.GetOneBook;

public record GetOneBookCommand([Required] int Id) : IRequest<Result<GetOneBookResponse>>, ICacheableQuery
{
    public string CacheKey => $"Book_{Id}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}