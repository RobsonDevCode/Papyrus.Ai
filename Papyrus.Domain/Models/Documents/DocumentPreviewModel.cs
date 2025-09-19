namespace Papyrus.Domain.Models.Documents;

public record DocumentPreviewModel
{
    public required Guid DocumentGroupId { get; init; }
    
    public required string FrontCoverImageUrl { get; set; }
    
    public required string Name { get; init; }
    
    public string? Author { get; init; }
    
    public required int TotalPages { get; init; }
    
    public DateTime CreatedAt { get; init; }
}