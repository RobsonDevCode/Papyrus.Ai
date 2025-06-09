namespace Papyrus.Api.Contracts.Contracts.Responses;

public record NoteResponse
{
    public required Guid NoteId { get; init; }

    public required Guid DocumentGroupId { get; init; }

    public required string Text { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; init; }
}