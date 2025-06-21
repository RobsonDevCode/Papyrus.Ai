namespace Papyrus.Api.Contracts.Contracts.Requests;

public record AddToNoteRequest
{
    public required Guid NoteId { get; init; }
    public required Guid DocumentId { get; init; }
    public required string Prompt { get; init; }
    public required int Page { get; init; }
}