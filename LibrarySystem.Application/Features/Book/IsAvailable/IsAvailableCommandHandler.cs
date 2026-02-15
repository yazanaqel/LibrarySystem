using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.IsAvailable;

internal class IsAvailableCommandHandler(IBookService bookService) : IRequestHandler<IsAvailableCommand,bool>
{
    private readonly IBookService _bookService = bookService;

    public async Task<bool> Handle(IsAvailableCommand request,CancellationToken cancellationToken)
    {
        return await _bookService.IsAvailable(request.bookId);
    }
}
