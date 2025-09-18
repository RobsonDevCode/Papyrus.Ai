namespace Papyrus.Ai.Configuration;

public class ElevenLabsClientSettings
{
    public required Uri BaseUrl { get; init; }
    public required string ApiKey { get; init; }
}