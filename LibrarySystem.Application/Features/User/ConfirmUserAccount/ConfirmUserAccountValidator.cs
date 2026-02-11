using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.ConfirmUserAccount;

internal class ConfirmUserAccountValidator : AbstractValidator<ConfirmUserAccountRequest>
{
    public ConfirmUserAccountValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
