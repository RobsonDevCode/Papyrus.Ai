
namespace Papyrus.Domain.Models.Audio;

public record AudioSettingsRequestModel
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    public required string VoiceId { get; init; }
    public required VoiceSettingModel VoiceSettings { get; init; }

    public DateTime? CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; }
}