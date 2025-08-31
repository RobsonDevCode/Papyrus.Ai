namespace Papyrus.Api.Contracts.Contracts.Requests;

public record WriteNoteRequest
{
    /// <summary>
    /// Page the note is on
    /// </summary>
   public Guid PageId { get; init; }
    
    /// <summary>
    /// The highlighted text to take a note on.
    /// </summary>
    public string? Text { get; init; } = null;
    
    /// <summary>
    /// The image selected to take a note on.
    /// </summary>
    public int? ImageReference  { get; init; } = null;

    /// <summary>
    ///  Used to prompt the llm to improve the given note
    /// </summary>
    public PromptRequest? Prompt { get; init; } = null;
}