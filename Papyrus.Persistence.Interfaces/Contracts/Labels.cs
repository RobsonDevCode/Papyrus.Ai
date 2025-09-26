namespace Papyrus.Persistance.Interfaces.Contracts;

public record Labels
{
    /// <summary>
    /// Voice accent (e.g., american, british, australian)
    /// </summary>
    public string Accent { get; init; }

    /// <summary>
    /// Descriptive style of the voice (e.g., casual, professional, dramatic)
    /// </summary>
    public string Descriptive { get; init; }

    /// <summary>
    /// Age category of the voice (e.g., young, middle_aged, old)
    /// </summary>
    public string Age { get; init; } 

    /// <summary>
    /// Gender of the voice (male, female, non_binary)
    /// </summary>
    public string Gender { get; init; }

    /// <summary>
    /// Language code (e.g., en, es, fr)
    /// </summary>
    public string Language { get; init; } 

    /// <summary>
    /// Use case category (e.g., conversational, audiobook, educational)
    /// </summary>
    public string UseCase { get; init; }
}