using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Perstistance.Interfaces.Contracts;

public record Note
{
    
    [BsonRepresentation(BsonType.String)]    
    public Guid DocumentId { get; init; } = Guid.NewGuid();

    [BsonId]
    [BsonRepresentation(BsonType.String)]    
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [BsonRepresentation(BsonType.String)]
    public required Guid DocumentGroupId { get; init; }
    
    public required string Text { get; init; }
    
    public required string Title { get; init; }
    
    public required int RelatedPage { get; init; }

    // indexes of the text used to make the note 
    public int? IndexStart { get; init; }
    
    public int? IndexEnd { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
}