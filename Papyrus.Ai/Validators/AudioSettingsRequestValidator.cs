using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class AudioSettingsRequestValidator : AbstractValidator<AudioSettingsRequest>
{
    public AudioSettingsRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
        
        RuleFor(x => x.VoiceId)
            .NotEmpty()
            .WithMessage("voiceId cannot be empty");

        RuleFor(x => x.VoiceSettings.Speed)
            .Must(x => x is > 0.6 and < 1.3)
            .WithMessage(
                "Invalid setting for speed received, expected to be greater or equal to 0.7 and less or equal to 1.2, received 1.5.");

        RuleFor(x => x.VoiceSettings.Stability)
            .Must(x => x is > 0 and <= 1)
            .WithMessage("stability must be greater or equal to 0.0 and less or equal to 1.0.");
        
        RuleFor(x => x.VoiceSettings.UseSpeakerBoost)
            .NotNull()
            .WithMessage("useSpeakerBoost cannot be empty");
    }
}