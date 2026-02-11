using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Repositories;

public interface IBorrowingService
{
    Task InsertBorrowingAsync(int userId,int bookId);
    Task DeleteBorrowingAsync(int userId,int bookId);
    Task<bool> IsAvailable(int id);
    Task<bool> IsBorrowedByMe(int userId,int bookId);
    Task<IEnumerable<Borrowing>> SelectAllUserBorrowingsAsync(int userId);
}
