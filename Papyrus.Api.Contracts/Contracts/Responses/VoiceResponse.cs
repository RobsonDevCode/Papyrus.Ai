namespace Papyrus.Api.Contracts.Contracts.Responses;

public record VoiceResponse
{
    public required string VoiceId { get; init; }
    
    public required string Name { get; init; }
    
    public string? Category { get; init; }
    
    public string? Description { get; init; }
    
    public string? PreviewUrl { get; init; }
    
    public VoiceSettings? Settings { get; init; }
    
    public LabelsResponse? Labels { get; init; }
    
    public bool IsFavourited  { get; init; }
    
    public bool IsSelected  { get; init; }
    
    public int? CreatedAtUnix { get; init; }
}