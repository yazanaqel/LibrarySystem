using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Application.Features.Book.Delete;

public record DeleteBookCommand([Required] int Id) :IRequest;
