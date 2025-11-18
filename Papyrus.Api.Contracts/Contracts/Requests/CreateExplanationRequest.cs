namespace Papyrus.Api.Contracts.Contracts.Requests;

public record CreateExplanationRequest
{
    public Guid DocumentGroupId { get; init; }
    public int PageNumber { get; init; }
    public string TextToExplain { get; init; }
}