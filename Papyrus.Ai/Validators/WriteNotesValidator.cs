using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class WriteNotesValidator : AbstractValidator<WriteNoteRequest>
{
    public WriteNotesValidator()
    {
        RuleFor(x => x.PageId).NotEmpty()
            .WithMessage("documentGroupId is required.");
        
        RuleFor(x => x.Text).Must(PassLengthCheckIfNotEmpty)
            .WithMessage("text must be between 1 and 500 characters.");

        RuleFor(x => x.Prompt)
            .Must(BeValidPrompt);
    }

    private bool PassLengthCheckIfNotEmpty(string? text)
    {
        if(string.IsNullOrWhiteSpace(text))
            return true;

        return text.Length is > 1 and <= 500;
    }

    private bool BeValidPrompt(PromptRequest? request)
    {
        if (request is null)
        {
            return true;
        }
        
        return request.Text.Length is > 1 and <= 500 && request.NoteId != Guid.Empty;
    }
    
}