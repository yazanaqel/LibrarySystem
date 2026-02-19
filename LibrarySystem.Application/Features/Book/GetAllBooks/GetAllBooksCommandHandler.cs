using LibrarySystem.Domain.Repositories;
using LibrarySystem.Domain.Shared;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

internal sealed class GetAllBooksCommandHandler(IBookService bookService) : IRequestHandler<GetAllBooksCommand,Result<List<GetAllBooksResponse>>>
{
    private readonly IBookService _bookService = bookService;

    public async Task<Result<List<GetAllBooksResponse>>> Handle(GetAllBooksCommand request,CancellationToken cancellationToken)
    {
        try
        {
            var booksList = await _bookService.SelectAllBooksAsync();

            if(booksList is null || booksList.Count() == 0)
            {
                return Result<List<GetAllBooksResponse>>.Failure("Not Found!");
            }

            return Result<List<GetAllBooksResponse>>.Success(booksList.Select(book => new GetAllBooksResponse
            (
                book.Id,
                book.Title,
                book.ImageURL ?? string.Empty

            )).ToList());
        }
        catch(Exception)
        {
            return Result<List<GetAllBooksResponse>>.Failure("Error");
        }
    }
}
