using LibrarySystem.Application.Features.Book.GetAllBooks;
using LibrarySystem.Domain.Repositories;
using LibrarySystem.Domain.Shared;
using MediatR;

namespace LibrarySystem.Application.Features.Book.GetOneBook;

internal sealed class GetOneBookCommandHandler(IBookService bookService) : IRequestHandler<GetOneBookCommand,Result<GetOneBookResponse>>
{
    private readonly IBookService _bookService = bookService;

    public async Task<Result<GetOneBookResponse>> Handle(GetOneBookCommand request,CancellationToken cancellationToken)
    {

        try
        {
            var book = await _bookService.SelectBookAsync(request.Id);

            if(book is null)
            {
                return Result<GetOneBookResponse>.Failure("Not Found!");
            }

            return Result<GetOneBookResponse>.Success(
                new GetOneBookResponse(
                                book.Id,
                                book.Title,
                                book.Author,
                                book.Description,
                                book.ISBN,
                                book.ImageURL ?? string.Empty,
                                book.IsAvailable));

        }
        catch(Exception)
        {
            return Result<GetOneBookResponse>.Failure("Error");
        }

    }
}
