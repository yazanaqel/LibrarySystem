namespace LibrarySystem.Application.Features.Book.GetOneBook;

public record GetOneBookResponse(int Id,string Title,string Author,string Description,string ISBN,string ImageUrl,bool IsAvilable);
