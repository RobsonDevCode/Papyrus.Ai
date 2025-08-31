namespace Papyrus.Persistance.Interfaces.Contracts;

public record PaginationOptions
{
    public int Page { get; init; } = 1;

    public int Size { get; init; } = 100;

}