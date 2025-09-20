namespace Papyrus.Api.Contracts.Contracts.Responses;

public record DocumentResponse
{
    public Guid DocumentGroupId { get; init; }

    public required string Name { get; init; }

    public string? FrontCoverImageUrl { get; init; }
    
    public string? Author { get; init; }

    public required int TotalPages { get; init; }
    
    public DateTime CreatedAt { get; init; }
}