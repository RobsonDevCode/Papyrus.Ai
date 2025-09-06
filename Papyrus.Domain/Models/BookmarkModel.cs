namespace Papyrus.Domain.Models;

public record BookmarkModel
{
    public Guid Id { get; init; }
    
    public Guid DocumentGroupId { get; init; }
    
    public int Page { get; init; }
    
    public DateTime CreatedAt { get; init; }
}