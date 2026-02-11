using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetOneBook;

internal sealed class GetOneBookCommandHandler(IBookService bookService) : IRequestHandler<GetOneBookCommand,GetOneBookResponse>
{
    private readonly IBookService _bookService = bookService;

    public async Task<GetOneBookResponse> Handle(GetOneBookCommand request,CancellationToken cancellationToken)
    {
        var book = await _bookService.SelectBookAsync(request.Id);

        return new GetOneBookResponse
        (
            book.Id,
            book.Title,
            book.Author,
            book.Description,
            book.ISBN,
            book.ImageURL ?? string.Empty
        );
    }
}
