namespace Papyrus.Api.Contracts.Contracts.Api;

public record Pagination
{
    public Pagination(int pageNumber, int pageSize, int total)
    {
        Page = pageNumber;
        Size = pageSize;
        Total = total;
    }
    
    public int Page { get; init; }
    public int Size { get; init; }
    public int Total { get; init; } 
}