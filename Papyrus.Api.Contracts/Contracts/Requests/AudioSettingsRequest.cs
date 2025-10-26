namespace Papyrus.Api.Contracts.Contracts.Requests;

public record AudioSettingsRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public required string VoiceId { get; init; }
    public required VoiceSettings VoiceSettings { get; init; }
}