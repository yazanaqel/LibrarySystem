using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.ConfirmUserAccount;

public record ConfirmUserAccountCommand(ConfirmUserAccountRequest Request) : IRequest<bool>;
