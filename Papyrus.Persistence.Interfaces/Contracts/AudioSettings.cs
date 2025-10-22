using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Contracts;

public sealed record AudioSettings
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required Guid Id { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid? UserId { get; init; }
    
    public required string VoiceId { get; init; }
    
    public required VoiceSetting VoiceSetting { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}