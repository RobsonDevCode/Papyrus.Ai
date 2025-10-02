namespace Papyrus.Api.Contracts.Contracts.Api;

public record PagedResponse<T>
{
    public T[] Items { get; init; }

    public required Pagination Pagination { get; init; }
}