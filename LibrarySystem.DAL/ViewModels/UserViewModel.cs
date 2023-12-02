using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.ViewModels
{
	public class UserViewModel
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
		[Compare("Password")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Role { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Token { get; set; } = string.Empty;

		public bool KeepLoggedIn { get; set; }
	}
}
