using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Audio;

public record CreateTextToSpeechModel
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    
    [JsonPropertyName("voice_settings")]
    public required VoiceSettingModel VoiceSettings { get; init; }
}