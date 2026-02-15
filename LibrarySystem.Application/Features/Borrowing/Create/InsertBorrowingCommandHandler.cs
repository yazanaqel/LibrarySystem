using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.Create;

internal class InsertBorrowingCommandHandler(IBorrowingService borrowingService) : IRequestHandler<InsertBorrowingCommand>
{
    private readonly IBorrowingService _borrowingService = borrowingService;

    public async Task Handle(InsertBorrowingCommand request,CancellationToken cancellationToken)
    {
        Domain.Entities.Borrowing borrowing = new Domain.Entities.Borrowing
        {
            UserId = request.userId,
            BookId = request.bookId
        };

        await _borrowingService.InsertBorrowingAsync(borrowing);
    }
}
