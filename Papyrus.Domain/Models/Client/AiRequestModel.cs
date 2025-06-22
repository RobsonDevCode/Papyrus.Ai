using System.Text.Json.Serialization;
using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Models.Client;

public record AiRequestModel
{
    [JsonPropertyName("contents")]
    public required List<Content> Contents { get; init; }

    [JsonPropertyName("generationConfig")]
    public GenerationConfig? GenerationConfig { get; set; }
}