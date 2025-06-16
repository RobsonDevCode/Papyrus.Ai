using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class WriteImageNoteValidator : AbstractValidator<WriteImageNoteRequest>
{
    public WriteImageNoteValidator()
    {
        RuleFor(x => x.DocumentGroupId).NotEmpty()
            .WithMessage("documentGroupId is required!");
        
        RuleFor(x => x.ImageReference).NotEmpty()
            .WithMessage("imageReference is required!");
        
        RuleFor(x => x.Page).NotEmpty()
            .WithMessage("page is required!");
    }
}