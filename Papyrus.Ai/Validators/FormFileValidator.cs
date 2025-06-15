using FluentValidation;

namespace Papyrus.Ai.Validators;

public class FormFileValidator : AbstractValidator<FormFile>
{
    public FormFileValidator()
    {
        RuleFor(x => x.Length).LessThanOrEqualTo(300 * 1024 * 1024)
            .WithMessage("Document length must be less than or equal to 300Mb.");
        
        RuleFor(x => x.Length).NotEmpty().WithMessage("File is required");
        RuleFor(x => x.Length).GreaterThan(0).WithMessage("File must have text length greater than 0");
        RuleFor(x => x).NotEmpty().WithMessage("File is required");
    }
}