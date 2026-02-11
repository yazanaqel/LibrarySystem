using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

public class BookService(ApplicationDbContext applicationDbContext) : IBookService
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task DeleteBookAsync(int id)
    {
        await _applicationDbContext.Books
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task InsertBookAsync(Book book)
    {
        _applicationDbContext.Books.Add(book);

        await _applicationDbContext.SaveChangesAsync();
    }

    public List<Book> SearchByAuthor(string author)
    {
        throw new NotImplementedException();
    }

    public List<Book> SearchByIsbn(string isbn)
    {
        throw new NotImplementedException();
    }

    public List<Book> SearchByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Book>> SelectAllBooksAsync() 
        => await _applicationDbContext.Books.AsNoTracking().ToListAsync();

    public async Task<Book> SelectBookAsync(int id) 
        => await _applicationDbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

    public async Task UpdateBookAsync(Book book)
    {
        await _applicationDbContext.Books
            .Where(b => b.Id == book.Id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(b => b.Title,book.Title)
                    .SetProperty(b => b.Author,book.Author)
                    .SetProperty(b => b.Description,book.Description)
                    .SetProperty(b => b.ImageURL,book.ImageURL));


    }
}
