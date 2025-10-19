using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record Bookmark
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public required Guid DocumentGroupId { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid? UserId { get; init; }
    
    public required int Page { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}