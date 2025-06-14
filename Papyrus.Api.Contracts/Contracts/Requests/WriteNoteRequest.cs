namespace Papyrus.Api.Contracts.Contracts.Requests;

public record WriteNoteRequest
{
    public Guid DocumentGroupId { get; init; }
    public int Page { get; init; }

    public string? Text { get; init; } = null;
}