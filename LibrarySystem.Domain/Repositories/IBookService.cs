using LibrarySystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Domain.Repositories;

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
