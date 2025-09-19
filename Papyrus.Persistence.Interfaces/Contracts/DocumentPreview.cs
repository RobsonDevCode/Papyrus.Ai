using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record DocumentPreview
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required Guid DocumentGroupId { get; init; }
    
    public string? FrontCoverImageUrl { get; init; }

    public required string Name { get; init; }

    public string? Author { get; init; }
    
    public required int TotalPages { get; init; } 
    
    public DateTime CreatedAt { get; init; }
}