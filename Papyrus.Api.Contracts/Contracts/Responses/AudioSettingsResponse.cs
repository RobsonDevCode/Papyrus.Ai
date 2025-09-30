namespace Papyrus.Api.Contracts.Contracts.Responses;

public record AudioSettingsResponse
{
    public required Guid Id { get; init; }
    
    public required string VoiceId { get; init; }
    
    public required VoiceSettings VoiceSettings { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required DateTime UpdatedAt { get; init; }
}