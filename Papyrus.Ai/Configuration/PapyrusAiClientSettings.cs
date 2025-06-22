namespace Papyrus.Ai.Configuration;

public record PapyrusAiClientSettings
{
    public required Uri BaseUrl { get; init; }
    public required string ApiKey { get; init; }
}