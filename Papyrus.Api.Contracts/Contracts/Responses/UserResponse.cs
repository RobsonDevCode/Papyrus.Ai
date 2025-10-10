namespace Papyrus.Api.Contracts.Contracts.Responses;

public record UserResponse
{
    public required Guid Id { get; init; }
    
    public required string Username { get; init; }
    
    public string? Name { get; init; }
    
    public string? Email { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required DateTime UpdatedAt { get; init; }
}