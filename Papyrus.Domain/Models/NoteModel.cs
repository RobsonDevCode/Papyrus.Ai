namespace Papyrus.Domain.Models;

public record NoteModel
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid DocumentGroupId { get; init; }
    public required string Note { get; init; }
    public required int PageReference  { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}