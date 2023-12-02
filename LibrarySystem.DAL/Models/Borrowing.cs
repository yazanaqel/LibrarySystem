using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models;

[Table("Borrowings")]
public class Borrowing
{

	[ForeignKey("User")]
	public int UserId { get; set; }


	[ForeignKey("Book")]
	public int BookId { get; set; }

}
