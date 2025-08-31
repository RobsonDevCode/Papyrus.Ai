using System.Text.Json.Serialization;
using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Models.Client;

public record AiRequestModel
{
    [JsonPropertyName("system_instruction")]
    public required SystemInturctions SystemInstruction { get; init; }
    
    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; }

    [JsonPropertyName("generationConfig")]
    public GenerationConfig? GenerationConfig { get; set; }
}