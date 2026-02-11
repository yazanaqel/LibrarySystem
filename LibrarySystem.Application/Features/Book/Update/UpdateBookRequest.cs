using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Features.Book.Update;

public record UpdateBookRequest(int Id,string Title,string Author,string ISBN,string Description,IFormFile? ImageURL);
