using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

internal class BorrowingService(ApplicationDbContext applicationDbContext) : IBorrowingService
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task DeleteBorrowingAsync(Borrowing borrowing)
    {
        var strategy = _applicationDbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            await _applicationDbContext.Borrowings
                .Where(b => b.UserId == borrowing.UserId && b.BookId == borrowing.BookId)
                .ExecuteDeleteAsync();

            await _applicationDbContext.Books
                .Where(i => i.Id == borrowing.BookId)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsAvailable,true));

            await transaction.CommitAsync();
        });
    }

    public async Task InsertBorrowingAsync(Borrowing borrowing)
    {
        _applicationDbContext.Borrowings.Add(borrowing);

        await _applicationDbContext.Books
            .Where(i => i.Id == borrowing.BookId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsAvailable,false));

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsBorrowedByMe(Borrowing borrowing)
    {
        return await _applicationDbContext.Borrowings.AnyAsync(b => b.UserId == borrowing.UserId && b.BookId == borrowing.BookId);
    }

    public async Task<IEnumerable<Book>> SelectAllUserBorrowingsAsync(int userId)
    {
        var booksList = await _applicationDbContext.Borrowings
            .Where(u => u.UserId == userId).Select(b => b.BookId)
            .Join(_applicationDbContext.Books,
                (bookId) => bookId,
                (book) => book.Id,
                (bookId,book) => book).ToListAsync();

        return booksList;
    }
}
