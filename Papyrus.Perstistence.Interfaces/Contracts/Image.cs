using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Perstistance.Interfaces.Contracts;

public record Image
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    
    public required string Bytes { get; init; }
    public float Width { get; init; }
    
    public float Height { get; init; }
    
    public int PageNumber { get; init; }
}