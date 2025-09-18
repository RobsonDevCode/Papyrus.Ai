namespace Papyrus.Api.Contracts.Contracts.Requests;

public record AudioSettingsRequest
{
    public required Guid Id { get; init; }
    public string VoiceId { get; init; }
    public VoiceSettings VoiceSettings { get; init; }
}