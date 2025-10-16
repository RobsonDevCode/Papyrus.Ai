using FluentValidation;
using Papyrus.Api.Contracts.Contracts.Requests;

namespace Papyrus.Ai.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");
        
        RuleFor(x => x.Username)
            .Must(IsValidUsername)
            .WithMessage("username must be between 3 and 40 characters");

        RuleFor(x => x)
            .Must(x => ValidateEmailOrUsername(x.Username, x.Email))
            .WithMessage("Either username or password has to be provided");


        RuleFor(x => x.Password)
            .Must(ValidatePassword)
            .WithMessage("Invalid password");
    }


    private bool IsValidUsername(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return true;
        }
        
        return username.Length > 3 
               && username.Length < 40 &&
               !username.Any(x => SpecialChars.Contains(x));
    }

    private bool ValidateEmailOrUsername(string? username, string? email)
    {
        return !(string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email));
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