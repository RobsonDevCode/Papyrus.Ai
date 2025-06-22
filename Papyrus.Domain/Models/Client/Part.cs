using System.Text.Json.Serialization;
using Papyrus.Domain.Models.Client;

namespace Papyrus.Domain.Clients;

public record Part
{
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    
    [JsonPropertyName("inlineData")]
    public InlineData InlineData { get; init; }
}