using LibrarySystem.DAL.Models;

namespace LibrarySystem.BLL.Services.BorrowingService;
public interface IBorrowingService
{
	Task InsertBorrowingAsync(Borrowing borrowing);
	Task DeleteBorrowingAsync(int userId,int bookId);
	Task<bool> IsAvailable(int id);
	Task<bool> IsBorrowedByMe(int userId,int bookId);
	Task<IEnumerable<Borrowing>> SelectAllUserBorrowingsAsync(int userId);
}
