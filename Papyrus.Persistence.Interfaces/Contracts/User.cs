using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record User
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required Guid Id { get; init; }

    public required string Username { get; init; }
    
    public string? Name { get; init; }
    
    public required string Email { get; init; }
    
    public required string PasswordHash { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required DateTime UpdatedAt { get; init; }
}