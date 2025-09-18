using Papyrus.Domain.Services.Interfaces.AudioBook;

namespace Papyrus.Domain.Models.Audio;

public record AudioSettingsModel
{
    public required Guid Id { get; init; }
    public required string VoiceId { get; init; }
    public required VoiceSettingModel VoiceSettings { get; init; }
}