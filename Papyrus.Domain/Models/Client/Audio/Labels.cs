using System.Text.Json.Serialization;

namespace Papyrus.Domain.Models.Client.Audio;

public record Labels
{
    /// <summary>
    /// Voice accent (e.g., american, british, australian)
    /// </summary>
    [JsonPropertyName("accent")]
    public string Accent { get; init; }

    /// <summary>
    /// Descriptive style of the voice (e.g., casual, professional, dramatic)
    /// </summary>
    [JsonPropertyName("descriptive")]
    public string Descriptive { get; init; }

    /// <summary>
    /// Age category of the voice (e.g., young, middle_aged, old)
    /// </summary>
    [JsonPropertyName("age")]
    public string Age { get; init; } 

    /// <summary>
    /// Gender of the voice (male, female, non_binary)
    /// </summary>
    [JsonPropertyName("gender")]
    public string Gender { get; init; }

    /// <summary>
    /// Language code (e.g., en, es, fr)
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; init; } 

    /// <summary>
    /// Use case category (e.g., conversational, audiobook, educational)
    /// </summary>
    [JsonPropertyName("use_case")]
    public string UseCase { get; init; }
}