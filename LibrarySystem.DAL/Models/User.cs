using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models;

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
	public string Password { get; set; } = string.Empty;

	[Required]
	[MaxLength(100)]
	public string Role { get; set; } = string.Empty;

	public string Token { get; set; } = string.Empty;

	public bool IsConfirmed { get; set; }
	public bool IsWrongPassword { get; set; }


}
