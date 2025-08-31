namespace Papyrus.Api.Contracts.Contracts.Requests;

public record EditNoteRequest
{
    /// <summary>
    /// Note identifier 
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// The manual improvement to the note
    /// </summary>
    public required string EditedNote { get; init; }
}