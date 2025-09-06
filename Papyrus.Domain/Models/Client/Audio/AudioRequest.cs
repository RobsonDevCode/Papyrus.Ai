using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client.Audio;

public record AudioRequest
{
    [JsonPropertyName("text")]
    public string Text { get; init; }
    
    [JsonPropertyName("model_id")]
    public string ModelId { get; init; }
}