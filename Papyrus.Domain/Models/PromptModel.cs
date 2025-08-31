namespace Papyrus.Domain.Models;

public record PromptModel
{
    public required Guid Id { get; init; }
    
    public required Guid ChatId { get; init; }

    public required Guid NoteId { get; init; }

    public required string Prompt { get; init; }

    public string? Response { get; init; }

    public DateTime CreatedAt { get; init; }
}
