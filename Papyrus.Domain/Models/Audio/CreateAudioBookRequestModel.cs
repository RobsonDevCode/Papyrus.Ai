
namespace Papyrus.Domain.Models.Audio;

public record CreateAudioBookRequestModel
{
    public required Guid DocumentGroupId { get; init; }
    public required Guid UserId { get; init; }
    public required string VoiceId { get; init; }
    public required string Text { get; init; }
    public required VoiceSettingModel VoiceSettings { get; init; }
    public required int[] Pages { get; init; }
}
