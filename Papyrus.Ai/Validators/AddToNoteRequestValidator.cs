using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class AddToNoteRequestValidator : AbstractValidator<AddToNoteRequest>
{
    public AddToNoteRequestValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty()
            .WithMessage("documentId is required");
        
        RuleFor(x => x.Page).NotEmpty()
            .WithMessage("page is required");
        
        RuleFor(x => x.NoteId).NotEmpty()
            .WithMessage("noteId is required");
        
        RuleFor(x => x.Prompt).NotEmpty()
            .WithMessage("prompt is required");

        RuleFor(x => x.Prompt).MaximumLength(1000)
            .WithMessage("prompt must not exceed 1000 characters");

        RuleFor(x => x.Prompt).MinimumLength(2)
            .WithMessage("prompt must be longer than 2 characters");
    }
}