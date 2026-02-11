using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.Book.Create;

public record CreateBookRequest(string Title,string Author, string ISBN, string Description, IFormFile? ImageURL);