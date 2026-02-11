using FluentValidation;

namespace LibrarySystem.Application.Features.Book.Create;

internal class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
    }
}
