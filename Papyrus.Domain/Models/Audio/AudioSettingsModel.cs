namespace Papyrus.Domain.Models.Audio;

public record AudioSettingsModel
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    public required string VoiceId { get; init; }
    
    public required VoiceSettingModel VoiceSettings { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required DateTime UpdatedAt { get; init; }
}