using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.PL.Models;

public class User
{

    public int Id { get; set; }


    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]

    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;


    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public bool IsConfirmed { get; set; }
    public bool IsWrongPassword { get; set; }
    public bool KeepLoggedIn { get; set; }


}