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
            .Must(ValidateUsername)
            .WithMessage("username must be between 3 and 40 characters");
        
        RuleFor(x => x.Name)
            .Must(ValidateName)
            .WithMessage("name must be between 3 and 40 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .Must(ValidatePassword)
            .WithMessage("Invalid password");
    }

    private bool ValidateUsername(string username)
    {
        return username.Length > 3 
               && username.Length < 40 &&
               !username.Any(x => SpecialChars.Contains(x));
    }

    private bool ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return true;
        }
        
        return name.Length > 3 && name.Length <= 40;
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