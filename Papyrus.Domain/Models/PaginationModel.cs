namespace Papyrus.Domain.Models;

public record PaginationModel
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
}