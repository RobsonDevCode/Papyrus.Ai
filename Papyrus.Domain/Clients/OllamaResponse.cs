using System.Text.Json.Serialization;

namespace Papyrus.Domain.Clients;

public record OllamaResponse
{
    [JsonPropertyName("response")]
    public string Repsonse  { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
}