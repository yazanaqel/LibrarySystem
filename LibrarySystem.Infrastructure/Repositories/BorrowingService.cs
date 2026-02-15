using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

internal class BorrowingService(ApplicationDbContext applicationDbContext) : IBorrowingService
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task DeleteBorrowingAsync(Borrowing borrowing)
    {
        await _applicationDbContext.Borrowings
            .Where(b => b.UserId == borrowing.UserId && b.BookId == borrowing.BookId)
            .ExecuteDeleteAsync();
    }

    public async Task InsertBorrowingAsync(Borrowing borrowing)
    {
        _applicationDbContext.Borrowings.Add(borrowing);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsBorrowedByMe(Borrowing borrowing)
    {
        return await _applicationDbContext.Borrowings.AnyAsync(b => b.UserId == borrowing.UserId && b.BookId == borrowing.BookId);
    }

    public Task<IEnumerable<Book>> SelectAllUserBorrowingsAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
