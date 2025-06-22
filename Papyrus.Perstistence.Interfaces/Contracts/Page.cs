
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Perstistance.Interfaces.Contracts;

public record Page
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid DocumentId { get; init; } = Guid.NewGuid();
    
    [BsonRepresentation(BsonType.String)]
    public Guid DocumentGroupId { get; init; }
    
    public required string DocumentName { get; init; }
    
    public string? Content { get; init; }
    
    public required int PageNumber { get; init; }
    
    public string Author { get; init; }
    
    public int ImageCount { get; init; }
    
    public string? PageImage { get; set; }
    
    public required bool IsImageOnly { get; init; }
    
    public required string Type { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}