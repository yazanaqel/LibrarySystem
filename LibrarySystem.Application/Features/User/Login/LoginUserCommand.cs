using LibrarySystem.Application.Features.User.Register;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Login;

public record LoginUserCommand(LoginUserRequest Request) : IRequest<Domain.Entities.User>;
