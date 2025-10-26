using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class CreateAudioBookRequestValidator : AbstractValidator<CreateAudioBookRequest>
{
   public CreateAudioBookRequestValidator()
   {
      RuleFor(x => x.DocumentGroupId).NotEmpty()
         .WithMessage("documentGroupId is required");
      
      RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("userId is required");
      
      RuleFor(x => x.VoiceId)
         .NotEmpty()
         .WithMessage("voiceId is required");

      RuleFor(x => x.VoiceId)
         .MinimumLength(10)
         .WithMessage("voiceId must be at least 10 characters invalid voice id given");
      
   }   
}