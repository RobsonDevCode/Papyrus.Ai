namespace Papyrus.Domain.Models;

public record ImageModel
{
    public required Guid Id { get; init; }
    
    public required Guid DocumentId { get; init; }
    public required string DocumentName{ get; init; }
    public required byte[] Bytes { get; init; }
}