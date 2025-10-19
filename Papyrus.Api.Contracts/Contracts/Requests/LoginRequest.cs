namespace Papyrus.Api.Contracts.Contracts.Requests;

public record LoginRequest
{
    public string Email { get; init; }
    
    public string Password { get; init; }
}