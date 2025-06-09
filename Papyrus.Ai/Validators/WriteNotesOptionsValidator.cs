using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class WriteNotesOptionsValidator : AbstractValidator<WriteNotesOptions>
{
    public WriteNotesOptionsValidator()
    {
        RuleFor(x => x).Must(HaveAtlestOneParam)
            .WithMessage("Either page or text is required!");
    }

    private bool HaveAtlestOneParam(WriteNotesOptions options)
    {
        return options.Page is not null || options.Text is not null;
    }
}