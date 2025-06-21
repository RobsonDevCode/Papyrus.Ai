namespace Papyrus.Api.Contracts.Contracts.Requests;

public record EditNoteRequest
{
    /// <summary>
    /// Note identifier 
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Document group identifier 
    /// </summary>
    public required Guid DocumentGroupId { get; init; }
    
    /// <summary>
    /// Page the note is on
    /// </summary>
    public int Page { get; init; }
    
    /// <summary>
    /// The manual improvement to the note
    /// </summary>
    public required string EditedNote { get; init; }
}