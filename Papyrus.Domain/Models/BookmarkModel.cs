namespace Papyrus.Domain.Models;

public record BookmarkModel
{
    public Guid Id { get; set; }
    public Guid DocumentGroupId { get; init; }
    public Guid? UserId { get; init; }
    public int Page { get; init; }
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}