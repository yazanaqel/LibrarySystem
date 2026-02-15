using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Repositories;

public interface IBookService
{
    Task InsertBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task<Book> SelectBookAsync(int id);
    Task<IEnumerable<Book>> SelectAllBooksAsync();
    Task<List<Book>> Search(string key,string value);
    Task<bool> IsAvailable(int id);

}
