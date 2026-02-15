using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.Delete;

public class DeleteBorrowingCommandHandler(IBorrowingService BorrowingService) : IRequestHandler<DeleteBorrowingCommand>
{
    private readonly IBorrowingService _borrowingService = BorrowingService;

    public async Task Handle(DeleteBorrowingCommand request,CancellationToken cancellationToken)
    {
        Domain.Entities.Borrowing borrowing = new Domain.Entities.Borrowing
        {
            UserId = request.userId,
            BookId = request.bookId
        };

        await _borrowingService.DeleteBorrowingAsync(borrowing);
    }
}
