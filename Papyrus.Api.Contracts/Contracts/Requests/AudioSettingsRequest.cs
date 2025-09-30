namespace Papyrus.Api.Contracts.Contracts.Requests;

public record AudioSettingsRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string VoiceId { get; init; }
    public required VoiceSettings VoiceSettings { get; init; }
}