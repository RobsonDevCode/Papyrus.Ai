namespace Papyrus.Domain.Models.Filters;

public record PaginationRequestModel
{
    public required int Page { get; init; }
    
    public required int Size { get; init; }
}