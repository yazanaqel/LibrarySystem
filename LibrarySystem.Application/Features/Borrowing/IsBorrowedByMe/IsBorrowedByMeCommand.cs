using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Features.Borrowing.IsBorrowedByMe;

public record IsBorrowedByMeCommand([Required] int userId,[Required] int bookId) : IRequest<bool>;
