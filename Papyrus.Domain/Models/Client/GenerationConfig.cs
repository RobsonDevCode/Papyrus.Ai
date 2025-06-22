using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public record GenerationConfig
{
    [JsonPropertyName("stopSequences")]
    public List<string>? StopSequences { get; set; }

    [JsonPropertyName("responseMimeType")]
    public string? ResponseMimeType { get; set; } // "text/plain", "application/json", "text/x.enum"

    [JsonPropertyName("responseSchema")]
    public string? ResponseSchema { get; set; } // JSON schema object
    
    [JsonPropertyName("thinkingConfig")]
    public ThinkingConfig ThinkingConfig { get; set; }
    [JsonPropertyName("candidateCount")]
    public int? CandidateCount { get; set; }

    [JsonPropertyName("maxOutputTokens")]
    public int? MaxOutputTokens { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("topP")]
    public double TopP { get; set; }

    [JsonPropertyName("topK")]
    public int? TopK { get; set; }

    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    [JsonPropertyName("presencePenalty")]
    public float? PresencePenalty { get; set; }

    [JsonPropertyName("frequencyPenalty")]
    public float? FrequencyPenalty { get; set; }

    [JsonPropertyName("responseLogprobs")]
    public bool? ResponseLogprobs { get; set; }

    [JsonPropertyName("logprobs")]
    public int? Logprobs { get; set; }
}