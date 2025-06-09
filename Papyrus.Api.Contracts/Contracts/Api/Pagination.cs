namespace Papyrus.Api.Contracts.Contracts.Api;

public record Pagination
{
    public int Page { get; init; } = 1;

    public int Size { get; init; } = 250;

    public required int Total { get; init; } = 0;
}