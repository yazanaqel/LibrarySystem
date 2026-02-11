using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.Register;

internal class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
    }
}
