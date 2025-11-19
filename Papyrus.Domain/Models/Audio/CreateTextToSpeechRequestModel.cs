namespace Papyrus.Domain.Models.Audio;

public record CreateTextToSpeechRequestModel
{
    public required Guid Id { get; init; }
    
    public required string Text { get; init; }

    public required string VoiceId { get; init; }
    public required VoiceSettingModel VoiceSettings { get; init; }
}