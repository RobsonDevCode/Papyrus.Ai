using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("email is required");
        
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