using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public record AiRequestModel
{
    [JsonPropertyName("model")]
    public required string Model { get; init; }
    
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    
    [JsonPropertyName("images")]
    public string[] ImageBytes { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; init; } = false;
    
    [JsonPropertyName("options")]
    public AiRequestOptionsModel Options { get; init; }
    
}