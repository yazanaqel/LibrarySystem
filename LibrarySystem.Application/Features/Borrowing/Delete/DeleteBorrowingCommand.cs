using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Features.Borrowing.Delete;

public record DeleteBorrowingCommand([Required] int userId,[Required] int bookId) : IRequest;
