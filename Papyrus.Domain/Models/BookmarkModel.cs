namespace Papyrus.Domain.Models;

public record BookmarkModel
{
    public required Guid Id { get; set; }
    public required Guid DocumentGroupId { get; init; }
    public required Guid UserId { get; init; }
    
    public required double Timestamp { get; init; }
    public required int Page { get; init; }
    public required DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}