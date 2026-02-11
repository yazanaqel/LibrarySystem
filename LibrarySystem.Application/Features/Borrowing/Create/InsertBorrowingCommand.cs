using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Application.Features.Borrowing.Create;

public record InsertBorrowingCommand([Required] int userId,[Required] int bookId) : IRequest;
