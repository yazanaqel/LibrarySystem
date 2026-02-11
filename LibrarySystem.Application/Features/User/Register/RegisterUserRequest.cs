using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Register;

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string ConfirmPassword);