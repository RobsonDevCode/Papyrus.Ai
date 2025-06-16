namespace Papyrus.Api.Contracts.Contracts.Requests;

public record WriteImageNoteRequest
{
    public required Guid DocumentGroupId { get; init; }
    public int ImageReference { get; init; }
    public int Page { get; set; }
}