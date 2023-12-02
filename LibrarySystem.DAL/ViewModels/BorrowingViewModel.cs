using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.ViewModels;
public class BorrowingViewModel
{
	public int BookId { get; set; }


	[Required]
	[MaxLength(100)]
	public string Title { get; set; } = string.Empty;


	[Required]
	[MaxLength(100)]
	public string Author { get; set; } = string.Empty;


	[Required]
	[MaxLength(20)]
	public string ISBN { get; set; } = string.Empty;


	[MaxLength(500)]
	public string Description { get; set; } = string.Empty;

	public string ImageURL { get; set; } = string.Empty;

	public bool IsAvilable { get; set; }
	public bool IsBorrowedByMe { get; set; }
}
