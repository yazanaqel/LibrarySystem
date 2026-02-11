using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.Book.GetAllBooks;

public record GetAllBooksResponse(int Id, string Title, string ImageURL);
