namespace Papyrus.Api.Contracts.Contracts.Api;

public record Pagination
{
    public int Page { get; init; } = 1;

    public int Size { get; init; } = 100;

    public required long Total { get; init; } 
}