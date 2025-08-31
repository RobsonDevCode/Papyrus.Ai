namespace Papyrus.Domain.Models.Filters;

public record NoteRequestModel
{
    public required Guid PageId { get; init; }
    public int? ImageReference { get; init; }
    public string? Text { get; init; }
    public PromptRequestModel? Prompt { get; set; }
}