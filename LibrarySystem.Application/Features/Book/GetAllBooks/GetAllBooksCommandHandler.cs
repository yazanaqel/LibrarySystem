using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

internal sealed class GetAllBooksCommandHandler(IBookService bookService) : IRequestHandler<GetAllBooksCommand,List<GetAllBooksResponse>>
{
    private readonly IBookService _bookService = bookService;

    public async Task<List<GetAllBooksResponse>> Handle(GetAllBooksCommand request,CancellationToken cancellationToken)
    {
        var booksList = await _bookService.SelectAllBooksAsync();

        return booksList.Select(book => new GetAllBooksResponse
        (
            book.Id,
            book.Title,
            book.ImageURL ?? string.Empty

        )).ToList();
    }
}
