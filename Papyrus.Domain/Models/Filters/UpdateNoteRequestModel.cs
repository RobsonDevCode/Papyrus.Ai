namespace Papyrus.Domain.Models.Filters;

public record UpdateNoteRequestModel
{
    public required Guid NoteId { get; init; }
    public required Guid DocumentId { get; init; }
    
    public required int Page { get; init; }
    public required string Prompt { get; init; }
}