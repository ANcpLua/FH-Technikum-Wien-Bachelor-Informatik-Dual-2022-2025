using FluentValidation;
using PaperlessServices.Entities;

namespace PaperlessServices.Validation;

public class BlValidation : AbstractValidator<BlDocument>
{
    public BlValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("'Name' must not be empty.")
            .MaximumLength(100)
            .WithMessage("'Name' must not exceed 100 characters.");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("'File Path' must not be empty.");

        RuleFor(x => x.DateUploaded)
            .NotEmpty()
            .WithMessage("'Date Uploaded' must not be empty.")
            .Must(date => date <= DateTime.UtcNow)
            .WithMessage("Upload date cannot be in the future.");
    }
}