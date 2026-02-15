using MediatR;

namespace LibrarySystem.Application.Features.Borrowing.GetUserBorrowing;

public record GetUserBorrowingCommand(int userId) : IRequest<List<GetUserBorrowingResponse>>;
