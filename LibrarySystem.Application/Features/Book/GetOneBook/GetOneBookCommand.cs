using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Features.Book.GetOneBook;

public record GetOneBookCommand([Required] int Id) : IRequest<GetOneBookResponse>;
