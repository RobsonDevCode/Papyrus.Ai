using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record Image
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public required Guid DocumentGroupId { get; init; }
    public required string DocumentName { get; init; }
    public required string Bytes { get; init; }
    public int PageNumber { get; init; }
}