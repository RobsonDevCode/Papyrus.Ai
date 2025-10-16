namespace Papyrus.Api.Contracts.Contracts.Requests;

public record CreateUserRequest
{
    public string Username { get; init; }
    
    public string Password { get; init; }
    
    public string Email { get; init; }
    
    public string? Name { get; init; }
}