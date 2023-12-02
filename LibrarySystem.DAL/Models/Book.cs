using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.DAL.Models;

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


	public IFormFile? File { get; set; }
	public string? ImageURL { get; set; } 


}