using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Repositories;

public interface IBorrowingService
{
    Task InsertBorrowingAsync(Borrowing borrowing);
    Task DeleteBorrowingAsync(Borrowing borrowing);
    Task<bool> IsBorrowedByMe(Borrowing borrowing);
    Task<IEnumerable<Book>> SelectAllUserBorrowingsAsync(int userId);
}
