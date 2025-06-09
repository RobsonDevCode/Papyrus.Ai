namespace Papyrus.Api.Contracts.Contracts.Requests;

public record WriteNotesOptions
{
    public int? Page { get; init; } = null;

    public string? Text { get; init; } = null;
}