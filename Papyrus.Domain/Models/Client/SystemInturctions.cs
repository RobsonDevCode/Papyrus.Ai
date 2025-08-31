using System.Text.Json.Serialization;
using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Models.Client;

public record SystemInturctions
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; init; }
}