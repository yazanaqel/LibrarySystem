using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;


public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Borrowing>()
            .HasKey(b => new { b.UserId,b.BookId });
    }

}
