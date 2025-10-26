namespace Papyrus.Domain.Models.Authentication;

public record JwtModel
{
    public required string AccessToken { get; init; }
    
    public required DateTime ExpiresAt { get; init; }
    
    public string? RefreshToken { get; init; }
}