using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class WriteNotesValidator : AbstractValidator<WriteNoteRequest>
{
    public WriteNotesValidator()
    {
        RuleFor(x => x.DocumentGroupId).NotEmpty()
            .WithMessage("documentGroupId is required.");
        
        RuleFor(x => x.Page).NotEmpty()
            .GreaterThan(0)
            .WithMessage("Page is required!");
        
    }
}