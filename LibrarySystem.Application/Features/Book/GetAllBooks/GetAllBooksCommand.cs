using MediatR;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

public record GetAllBooksCommand() : IRequest<List<GetAllBooksResponse>>;
