namespace Papyrus.Domain.Models.Client.Audio;

public record AudioWithAlignmentResult
{
    public Stream AudioStream { get; init; }
    
    public List<AlignmentDataModel> NormalizedAlignment { get; init; }
}