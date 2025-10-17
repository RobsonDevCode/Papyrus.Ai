namespace Papyrus.Api.Contracts.Contracts.Requests;

public record CreateBookmarkRequest
{
    public Guid? Id { get; init; }
    public required Guid? UserId { get; init; }
    public required Guid DocumentGroupId { get; init; }
    public required int Page { get; init; }
    public required DateTime CreateAt { get; init; }
}