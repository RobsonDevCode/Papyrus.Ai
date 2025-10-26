namespace Papyrus.Domain.Models.Authentication;

public record RefreshTokenModel
{
    public required string Token { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required DateTime ExpiresAt { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public bool IsRevoked { get; init; }
}