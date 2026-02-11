using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Register;

public record RegisterUserCommand(RegisterUserRequest Request) : IRequest;