namespace Papyrus.Domain.Models;

public record PageModel
{
    public Guid DocumentGroupId { get; init; }
    public required string DocumentName { get; init; }
    public string Author { get; init; }
    public required string Content { get; init; }
    public required int PageNumber { get; init; }
    public required string DocumentType { get; init; }
    
    public List<ImageModel>? Images { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}