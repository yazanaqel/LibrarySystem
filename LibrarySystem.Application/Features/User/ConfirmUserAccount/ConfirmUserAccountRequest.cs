using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.ConfirmUserAccount;

public sealed record ConfirmUserAccountRequest(string Email, string Token);
