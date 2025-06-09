namespace Papyrus.Domain.Models.Filters;

public record NoteRequestModel
{
    public required Guid DocumentTypeId { get; init; }

    public int? Page { get; init; } = null;

    public string? Text { get; init; } = null;
}