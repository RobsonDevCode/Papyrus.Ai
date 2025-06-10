using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public record AiRequestOptionsModel
{
    [JsonPropertyName("temperature")]
    public double Tempurature { get; init; } = 0.2;
    
    [JsonPropertyName("top_p")]
    public double TopP { get; init; } = 0.8;
}