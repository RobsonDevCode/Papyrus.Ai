using System.Text.Json.Serialization;
using Papyrus.Api.Contracts.Contracts;

namespace Papyrus.Domain.Models.Client.Audio;

public record VoiceResponseModel
{
    [JsonPropertyName("voice_id")]
    public string VoiceId { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("category")]
    public string? Category { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    [JsonPropertyName("preview_url")]
    public string? PreviewUrl { get; init; }
    
    [JsonPropertyName("settings")]
    public VoiceSettings? Settings { get; init; }
    
    [JsonPropertyName("is_mixed")]
    public bool IsMixed { get; init; }
    
    [JsonPropertyName("created_at_unix")]
    public int CreatedAtUnix { get; init; }
}