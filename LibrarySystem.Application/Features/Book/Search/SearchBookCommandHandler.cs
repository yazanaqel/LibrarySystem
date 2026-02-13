using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Book.Search;

internal class SearchBookCommandHandler(IBookService bookService) : IRequestHandler<SearchBookCommand,List<Domain.Entities.Book>>
{
    private readonly IBookService _bookService = bookService;

    public async Task<List<Domain.Entities.Book>> Handle(SearchBookCommand request,CancellationToken cancellationToken)
    {
        return await _bookService.Search(request.key,request.value);
    }
}
