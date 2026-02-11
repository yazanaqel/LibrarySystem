using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Features.User.GetUser;

public record GetUserCommand([EmailAddress] string Email) : IRequest<GetUserResponse>;
