namespace Papyrus.Domain.Models;

public record AudioBookModel
{
    public Guid DocumentGroupId { get; init; }
    
    public string VoiceId { get; init; }
    
    public byte[] AudioBytes { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}