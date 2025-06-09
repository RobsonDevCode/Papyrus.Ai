namespace Papyrus.Api.Contracts.Contracts.Api;

public record DocumentPaginationOptions
{
    public int Page { get; init; } = 1;
}