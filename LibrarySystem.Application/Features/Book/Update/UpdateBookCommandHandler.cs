using LibrarySystem.Application.Extensions;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.Update;

internal sealed class UpdateBookCommandHandler(IBookService bookService) : IRequestHandler<UpdateBookCommand>
{
    private readonly IBookService _bookService = bookService;

    public async Task Handle(UpdateBookCommand request,CancellationToken cancellationToken)
    {

        string imageUrl = string.Empty;

        if(request.Request.ImageURL is not null)
        {
            imageUrl = request.Request.ImageURL.Upload();
        }

        await _bookService.UpdateBookAsync(new Domain.Entities.Book
        {
            Id = request.Request.Id,
            Title = request.Request.Title,
            Author = request.Request.Author,
            ISBN = request.Request.ISBN,
            Description = request.Request.Description,
            ImageURL = imageUrl
        });
    }
}
