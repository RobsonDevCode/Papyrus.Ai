using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public record Error
{
    [JsonPropertyName("code")]
    public int Code { get; init; }
    
    [JsonPropertyName("message")]
    public string Message { get; init; }
}