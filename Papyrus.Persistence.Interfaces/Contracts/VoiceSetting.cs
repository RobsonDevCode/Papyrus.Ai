namespace Papyrus.Persistance.Interfaces.Contracts;

public record VoiceSetting
{
    public double Stability { get; init; }
    
    public bool UseSpeakerBoost  { get; init; }
    
    public double Speed  { get; init; }
}