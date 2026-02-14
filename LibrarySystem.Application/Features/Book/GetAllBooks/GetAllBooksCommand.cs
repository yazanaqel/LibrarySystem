using LibrarySystem.Domain.Shared;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

public record GetAllBooksCommand() : IRequest<Result<List<GetAllBooksResponse>>>;
