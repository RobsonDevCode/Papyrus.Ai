namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public record VoiceSettingModel
{
    public double Stability { get; init; }
    
    public bool UseSpeakerBoost  { get; init; }
    
    public double Speed  { get; init; }
}