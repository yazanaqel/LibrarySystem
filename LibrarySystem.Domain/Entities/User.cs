using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibrarySystem.Domain.Entities;

[Table("Users")]
public class User
{

    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;


    [Required]
    [MaxLength(100)]
    public byte[] Password { get; set; }
    public byte[] PasswordSalt { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public bool IsConfirmed { get; set; }
    public bool IsWrongPassword { get; set; }


}
