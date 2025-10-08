namespace Papyrus.Api.Contracts.Contracts.Responses;

public record AlignmentDataResponse
{
    public List<string> Characters { get; init; }
    
    public List<double> CharactersStartTimesSeconds { get; init; }
    
    public List<double> CharactersEndTimesSeconds { get; init; }
}