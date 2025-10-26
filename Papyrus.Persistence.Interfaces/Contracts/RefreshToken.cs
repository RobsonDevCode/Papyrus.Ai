using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Perstistance.Interfaces.Contracts;

public record RefreshToken
{

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init ; } = Guid.NewGuid();
    
    [BsonRepresentation(BsonType.String)]
    public required string Token { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public required Guid UserId { get; init; }
    
    public required DateTime ExpiresAt { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public bool IsRevoked { get; init; }
}