using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.GetUserBorrowing;

internal class GetUserBorrowingCommandHandler(IBorrowingService borrowingService) : IRequestHandler<GetUserBorrowingCommand,List<GetUserBorrowingResponse>>
{
    private readonly IBorrowingService _borrowingService = borrowingService;

    public async Task<List<GetUserBorrowingResponse>> Handle(GetUserBorrowingCommand request,CancellationToken cancellationToken)
    {
        var booksList = await _borrowingService.SelectAllUserBorrowingsAsync(request.userId);

        return booksList.Select(b => new GetUserBorrowingResponse(b.Id,b.Title,b.ImageURL)).ToList();
    }
}
