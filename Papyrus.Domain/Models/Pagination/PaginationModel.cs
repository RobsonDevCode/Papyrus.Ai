namespace Papyrus.Domain.Models.Pagination;

public record PaginationModel
{
    public required int Page { get; init; }
    
    public required int Size { get; init; }
    
    public int TotalPages { get; init; }
}