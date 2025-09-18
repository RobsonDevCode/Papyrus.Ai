namespace Papyrus.Api.Contracts.Contracts;

public record VoiceSettings
{
    public double Stability { get; init; }
    
    public bool UseSpeakerBoost  { get; init; }
    
    public double Speed  { get; init; }
}