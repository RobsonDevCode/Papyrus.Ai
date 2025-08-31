using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record Prompt
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid ChatId { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid NoteId { get; init; }
    
    public required string UserPrompt { get; init; }
    
    public required string Response { get; init; }
    
    public DateTime CreatedAt { get; init; }
}