using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Repositories;

namespace LibrarySystem.Infrastructure.Repositories;

internal class BorrowingService : IBorrowingService
{
    public Task DeleteBorrowingAsync(int userId,int bookId)
    {
        throw new NotImplementedException();
    }

    public Task InsertBorrowingAsync(int userId,int bookId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAvailable(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsBorrowedByMe(int userId,int bookId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Borrowing>> SelectAllUserBorrowingsAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
