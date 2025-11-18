namespace Papyrus.Api.Contracts.Contracts.Responses;

public record BookmarkResponse
{
    public required Guid Id { get; init; }
    
    public required Guid DocumentGroupId { get; init; }
    
    public required double Timestamp { get; init; }
    
    public required int Page { get; init; }
    
    public required DateTime CreateAt { get; init; }
}