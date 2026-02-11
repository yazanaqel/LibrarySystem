using FluentValidation;
using LibrarySystem.Application.Features.User.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Login;


internal class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
