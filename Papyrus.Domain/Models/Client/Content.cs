using System.Text.Json.Serialization;

namespace Papyrus.Domain.Clients;

public record Content
{
   [JsonPropertyName("role")]
   public string Role { get; init; }
   
   [JsonPropertyName("parts")]
   public required List<Part> Parts { get; set; } 
}