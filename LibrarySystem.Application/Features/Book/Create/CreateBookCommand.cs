using MediatR;

namespace LibrarySystem.Application.Features.Book.Create;

public record CreateBookCommand(CreateBookRequest Request) : IRequest;
