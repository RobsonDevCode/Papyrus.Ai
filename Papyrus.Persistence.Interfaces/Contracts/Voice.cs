using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Persistance.Interfaces.Contracts;

public record Voice
{
    [BsonId]
    public required string VoiceId { get; init; }
    
    public required string Name { get; init; }
    
    public string? Category { get; init; }
    
    public string? Description { get; init; }
    
    public string? PreviewUrl { get; init; }
    
    public Labels? Labels { get; init; }
    
    public VoiceSettings? Settings { get; init; }
    
    public bool IsMixed { get; init; }
    
    public int? CreatedAtUnix { get; init; }
}