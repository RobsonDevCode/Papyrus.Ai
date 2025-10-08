namespace Papyrus.Api.Contracts.Contracts.Responses;

public record AudioWithAlignmentResponse
{
    public required string AudioUrl { get; init; }
    
    public required List<AlignmentDataResponse> Alignment { get; init; }
}