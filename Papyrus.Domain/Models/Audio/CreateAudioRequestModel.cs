using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Audio;

public record CreateAudioRequestModel
{
    public required Guid DocumentGroupId { get; init; }
    
    public required string VoiceId { get; init; }
    
    public required int[] Pages { get; init; }
    
    public required string Text { get; init; }
    
    public required VoiceSettingModel VoiceSettings { get; init; }
}