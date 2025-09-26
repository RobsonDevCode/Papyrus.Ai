using Papyrus.Api.Contracts.Contracts;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Models;

public record VoiceModel
{
    public required string VoiceId { get; init; }
    
    public required string Name { get; init; }
    
    public string? Category { get; init; }
    
    public string? Description { get; init; }
    
    public string? PreviewUrl { get; init; }
    
    public VoiceSettings? Settings { get; init; }
    
    public Labels? Labels { get; init; }
    
    public bool IsFavourited  { get; init; }
    
    public bool IsSelected  { get; init; }
    
    public int? CreatedAtUnix { get; init; }
}