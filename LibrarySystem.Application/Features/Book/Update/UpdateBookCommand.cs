using MediatR;

namespace LibrarySystem.Application.Features.Book.Update;

public record UpdateBookCommand(UpdateBookRequest Request) : IRequest;
