namespace Papyrus.Persistance.Interfaces.Contracts;

public record AlignmentData
{
    public List<string> Charaters { get; init; }
    
    public List<double> CharacterStartTimesSeconds { get; init; }
    
    public List<double> CharacterEndTimesSeconds { get; init; }
}