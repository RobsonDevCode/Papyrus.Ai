using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("username cannot be empty");

        RuleFor(x => x.Username)
            .Must(x => x.Length > 3 && x.Length < 40)
            .WithMessage("username must be between 3 and 40 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .Must(ValidatePassword)
            .WithMessage("Invalid password");
    }


    private bool ValidatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < 12)
        {
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            return false;
        }

        return password.Any(c => SpecialChars.Contains(c));
    }
}