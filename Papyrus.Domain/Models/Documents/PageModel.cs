namespace Papyrus.Domain.Models.Documents;

public record PageModel
{
    public Guid DocumentGroupId { get; init; }
    public Guid ChatId { get; init; }
    
    public Guid? UserId { get; init; } //null for now i can be arsed to make a new model adding it in while i add users 
    public required string DocumentName { get; init; }
    
    public required string S3Key { get; init; }
    
    public string? Author { get; init; }
    
    public string? Content { get; init; }
    
    public required int PageNumber { get; init; }
    
    public required string DocumentType { get; init; }
    
    public required int ImageCount { get; init; }
    
    public string? ImageUrl { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}