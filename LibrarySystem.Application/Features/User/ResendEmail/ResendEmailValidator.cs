using FluentValidation;

namespace LibrarySystem.Application.Features.User.ResendEmail;

internal class ResendEmailValidator : AbstractValidator<ResendEmailRequest>
{
    public ResendEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
