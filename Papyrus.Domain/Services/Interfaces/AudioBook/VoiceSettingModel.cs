using System.Text.Json.Serialization;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public record VoiceSettingModel
{
    [JsonPropertyName("stability")]
    public double Stability { get; init; }
    
    [JsonPropertyName("use_speaker_boost")]
    
    public bool UseSpeakerBoost  { get; init; }
    
    [JsonPropertyName("speed")]
    public double Speed  { get; init; }
}