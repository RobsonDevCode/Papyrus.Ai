namespace Papyrus.Domain.Models.Filters;

public record PromptRequestModel
{
    public required Guid NoteId { get; init; }
    
    public string Text { get; init; }
}