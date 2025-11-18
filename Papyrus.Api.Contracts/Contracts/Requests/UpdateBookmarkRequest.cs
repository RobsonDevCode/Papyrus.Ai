namespace Papyrus.Api.Contracts.Contracts.Requests;

public record UpdateBookmarkRequest
{
    public Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required Guid DocumentGroupId { get; init; }
    
    public required int NewPage { get; init; }
}