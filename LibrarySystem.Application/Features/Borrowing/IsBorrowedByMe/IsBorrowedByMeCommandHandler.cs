using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.IsBorrowedByMe;

internal class IsBorrowedByMeCommandHandler(IBorrowingService borrowingService) : IRequestHandler<IsBorrowedByMeCommand,bool>
{
    private readonly IBorrowingService _borrowingService = borrowingService;

    public async Task<bool> Handle(IsBorrowedByMeCommand request,CancellationToken cancellationToken)
    {
        Domain.Entities.Borrowing borrowing = new Domain.Entities.Borrowing
        {
            UserId = request.userId,
            BookId = request.bookId
        };

        return await _borrowingService.IsBorrowedByMe(borrowing);
    }
}
