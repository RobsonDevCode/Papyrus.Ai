using System.Text.Json.Serialization;
using Papyrus.Domain.Models.Client;

namespace Papyrus.Domain.Clients;

public record LlmResponse
{
   [JsonPropertyName("candidates")]
   public Candidates[] Candidates { get; init; }
   
   [JsonPropertyName("responseId")]
   public string ResponseId { get; init; }
   
   [JsonIgnore]
   public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
   
   [JsonIgnore]
   public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
}