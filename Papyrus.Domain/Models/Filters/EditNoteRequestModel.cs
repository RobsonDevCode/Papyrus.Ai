namespace Papyrus.Domain.Models.Filters;

public record EditNoteRequestModel
{
    public required Guid Id { get; init; }
    public required Guid DocumentGroupId { get; init; }
    public int Page { get; init; }
    public required string EditedNote { get; init; }
}