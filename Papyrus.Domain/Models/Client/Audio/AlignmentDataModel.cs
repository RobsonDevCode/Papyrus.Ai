namespace Papyrus.Domain.Models.Client.Audio;

public record AlignmentDataModel
{
    public List<string> Charaters { get; init; }
    
    public List<double> CharacterStartTimesSeconds { get; init; }
    
    public List<double> CharacterEndTimesSeconds { get; init; }
}