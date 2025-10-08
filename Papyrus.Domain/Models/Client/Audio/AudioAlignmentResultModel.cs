namespace Papyrus.Domain.Models.Client.Audio;

public record AudioAlignmentResultModel
{
    public required string AudioUrl { get; init; }
    
    public required List<AlignmentDataModel> Alignment { get; init; }
}