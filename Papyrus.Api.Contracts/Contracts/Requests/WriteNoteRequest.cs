namespace Papyrus.Api.Contracts.Contracts.Requests;

public record WriteNoteRequest
{
    /// <summary>
    /// Identifier for the document. 
    /// </summary>
    public Guid DocumentGroupId { get; init; }
    
    /// <summary>
    /// The page in the document that's being reference. Note pdf's are 1 indexed
    /// </summary>
    public int Page { get; init; }
    
    /// <summary>
    /// The highlighted text to take a note on.
    /// </summary>
    public string? Text { get; init; } = null;
    
    /// <summary>
    /// The image selected to take a note on.
    /// </summary>
    public int? ImageReference  { get; init; } = null;
}