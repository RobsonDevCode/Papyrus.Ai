using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class EditNoteRequestValidator : AbstractValidator<EditNoteRequest>
{
    public EditNoteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
        
        RuleFor(x => x.EditedNote)
            .NotEmpty()
            .WithMessage("editedNote cannot be empty");
        
        RuleFor(x => x.EditedNote)
            .MinimumLength(1)
            .WithMessage("editedNote must be at least 1 character long");
        
        RuleFor(x => x.EditedNote)
            .MaximumLength(5000)
            .WithMessage("editedNote cant exceed 2000 characters, please try creating multiple notes");
    }
}