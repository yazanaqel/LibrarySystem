using LibrarySystem.Application.Extensions;
using LibrarySystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Features.Book.Create;

internal class CreateBookCommandHandler(IBookService bookService) : IRequestHandler<CreateBookCommand>
{
    private readonly IBookService _bookService = bookService;

    public async Task Handle(CreateBookCommand request,CancellationToken cancellationToken)
    {
        string imageUrl = string.Empty;

        if(request.Request.ImageURL is not null)
        {
            imageUrl = request.Request.ImageURL.Upload();
        }

        await _bookService.InsertBookAsync(new Domain.Entities.Book
        {
            Title = request.Request.Title,
            Author = request.Request.Author,
            ISBN = request.Request.ISBN,
            Description = request.Request.Description,
            ImageURL = imageUrl
        });
    }

}
