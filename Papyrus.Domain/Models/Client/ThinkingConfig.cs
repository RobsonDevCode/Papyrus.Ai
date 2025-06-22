using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public record ThinkingConfig
{
    [JsonPropertyName("includeThoughts")]
    public bool? IncludeThoughts { get; set; }
    
    [JsonPropertyName("thinkingBudget")]
    public int? ThinkingBudget { get; set; }
}