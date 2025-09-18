namespace Papyrus.Api.Contracts.Contracts.Requests;

public record CreateAudioBookRequest
{
    public Guid DocumentGroupId { get; init; }
    
    public string VoiceId { get; init; }
    
    public int[] Pages { get; init; }
}