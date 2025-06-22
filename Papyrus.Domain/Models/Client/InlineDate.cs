using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client;

public class InlineData
{
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set; } // base64 encoded
}