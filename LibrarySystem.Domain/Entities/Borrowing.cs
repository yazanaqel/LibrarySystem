using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities;

[Table("Borrowings")]
public class Borrowing
{

    [ForeignKey("User")]
    public int UserId { get; set; }


    [ForeignKey("Book")]
    public int BookId { get; set; }

}
