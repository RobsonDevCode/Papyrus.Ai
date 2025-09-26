using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client.Audio;

public record VoicesResponseModel
{
    [JsonPropertyName("voices")]
    public IEnumerable<VoiceResponseModel> Voices { get; init; }
    
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }
}