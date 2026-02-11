using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.ResendEmail;

public record ResendEmailCommand(ResendEmailRequest Request) : IRequest;