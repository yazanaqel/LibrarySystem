using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.BookService;
public interface IBookService
{
    Task InsertBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task<Book> SelectBookAsync(int id);
    Task<IEnumerable<Book>> SelectAllBooksAsync();
    List<Book> SearchByTitle(string title);
    List<Book> SearchByAuthor(string author);
    List<Book> SearchByIsbn(string isbn);
}
