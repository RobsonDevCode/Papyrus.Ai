using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class CreateExplanationTextToSpeechValidator : AbstractValidator<CreateExplanationTextToSpeechRequest>
{
    public CreateExplanationTextToSpeechValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
        
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("text cannot be empty");
        
        RuleFor(x => x.Text)
            .MaximumLength(1000)
            .WithMessage("text cannot exceed 1000 characters");
        
        RuleFor(x => x.VoiceId)
            .NotEmpty()
            .WithMessage("voiceId cannot be empty");
        
        RuleFor(x => x.VoiceSettings)
            .NotEmpty()
            .WithMessage("voiceSettings cannot be empty");
    }
    
    
}