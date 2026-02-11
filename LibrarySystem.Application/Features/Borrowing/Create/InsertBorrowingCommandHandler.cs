using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.Create;

internal class InsertBorrowingCommandHandler(IBorrowingService borrowingService) : IRequestHandler<InsertBorrowingCommand>
{
    private readonly IBorrowingService _borrowingService = borrowingService;

    public async Task Handle(InsertBorrowingCommand request,CancellationToken cancellationToken)
    {
        await _borrowingService.InsertBorrowingAsync(request.userId,request.bookId);
    }
}
