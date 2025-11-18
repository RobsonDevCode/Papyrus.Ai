namespace Papyrus.Domain.Models.Explanation;

public record CreateExplanationRequestModel
{
    public required Guid DocumentGroupId { get; init; }
    public required Guid UserId { get; init; }
    public required int PageNumber { get; init; }
    public required string TextToExplain { get; init; }
}