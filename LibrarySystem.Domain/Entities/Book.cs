using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibrarySystem.Domain.Entities;

[Table("Books")]
public class Book
{

    [Key]
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


    public string? ImageURL { get; set; }

    public bool IsAvailable { get; set; }
}