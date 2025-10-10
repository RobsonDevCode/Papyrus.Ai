namespace Papyrus.Domain.Models.User;

public record UserModel
{
    public required Guid Id { get; init; }
    
    public required string Username { get; init; }
    
    public required string Password { get; init; }
    
    public required string Email { get; init; }
    
    public string? Name { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required DateTime UpdatedAt { get; init; }
}