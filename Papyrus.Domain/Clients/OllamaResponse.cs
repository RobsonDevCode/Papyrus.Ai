using System.Text.Json.Serialization;

namespace Papyrus.Domain.Clients;

public record OllamaResponse
{
    [JsonPropertyName("response")]
    public string Repsonse  { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime Created { get; set; }
    
    [JsonPropertyName("done")]
    public bool Done { get; set; }
}