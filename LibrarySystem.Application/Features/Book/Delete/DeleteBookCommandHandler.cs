using LibrarySystem.Application.Extensions;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.Delete;

internal sealed class DeleteBookCommandHandler(IBookService bookService) : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookService _bookService = bookService;

    public async Task Handle(DeleteBookCommand request,CancellationToken cancellationToken)
    {
        var book = await _bookService.SelectBookAsync(request.Id);

        if(book is not null && !string.IsNullOrEmpty(book.ImageURL))
        {
            book.ImageURL.Delete();
        }


        await _bookService.DeleteBookAsync(request.Id);
    }
}
