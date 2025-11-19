namespace Papyrus.Api.Contracts.Contracts.Requests;

public class CreateExplanationTextToSpeechRequest
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    
    public string VoiceId { get; init; }
    public VoiceSettings VoiceSettings { get; init; }
}