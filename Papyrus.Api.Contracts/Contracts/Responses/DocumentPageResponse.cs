namespace Papyrus.Api.Contracts.Contracts.Responses;

public record DocumentPageResponse
{
    public required Guid DocumentGroupId { get; init; }
    public required string DocumentName { get; init; }
    public string? Author { get; init; }
    public required int PageNumber { get; init; }

    public required string DocumentType { get; init; }
    
    public required int ImageCount { get; init; }
        
    public required string? ImageUrl { get; init; }
    
    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; init; }
}