using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class CreateExplanationRequestValidator : AbstractValidator<CreateExplanationRequest>
{
    public CreateExplanationRequestValidator()
    {
        RuleFor(x => x.DocumentGroupId).NotEmpty()
            .WithMessage("documentGroupId cannot be empty");
        
        RuleFor(x => x.PageNumber).NotEmpty()
            .WithMessage("pageNumber cannot be empty");
        
        RuleFor(x => x.PageNumber)
            .Must(page => page > 0)
            .WithMessage("pageNumber must be positive");
            
        RuleFor(x => x.TextToExplain)
            .NotEmpty()
            .WithMessage("textToExplain cannot be empty");
        
        RuleFor(x => x.TextToExplain)
            .Must(textToExplain => textToExplain.Length > 1)
            .WithMessage("textToExplain has to be at least 2 characters");
        
        
    }
}