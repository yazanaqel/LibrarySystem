using MediatR;

namespace LibrarySystem.Application.Features.Book.IsAvailable;

public record IsAvailableCommand(int bookId) : IRequest<bool>;
