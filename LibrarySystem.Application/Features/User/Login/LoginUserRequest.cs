using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Login;


public sealed record LoginUserRequest(
    string Email,
    string Password,
    bool KeepLoggedIn);
