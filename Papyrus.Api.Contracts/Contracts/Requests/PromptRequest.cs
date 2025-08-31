namespace Papyrus.Api.Contracts.Contracts.Requests;

public record PromptRequest
{
    /// <summary>
    /// Identifier of the note
    /// </summary>
    public Guid NoteId { get; init; }
    
    
    /// <summary>
    /// Prompt used to edit note
    /// </summary>
    public string Text { get; init; } = null;
}