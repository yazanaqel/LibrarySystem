using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.PL.Models;

public class Book
{

    public int Id { get; set; }


    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;


    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;


    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;


    public IFormFile? File { get; set; }
    public string? ImageURL { get; set; }
    public bool IsAvilable { get; set; }
    public bool IsBorrowedByMe { get; set; }


}