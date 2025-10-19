using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class CreateBookmarkRequestValidator : AbstractValidator<CreateBookmarkRequest>
{
    public CreateBookmarkRequestValidator()
    {
        RuleFor(x => x.DocumentGroupId)
            .NotEmpty()
            .WithMessage("documentGroupId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("userId is required");
        
        RuleFor(x => x.Page)
            .NotEmpty()
            .WithMessage("page is required");
        
        RuleFor(x => x.Page)
            .Must(x => x > 0)
            .WithMessage("page must be a positive integer and greater than zero");
    }
}