namespace Papyrus.Api.Contracts.Contracts.Api;

public record Pagination
{
    public Pagination(int pageNumber, int pageSize, long total)
    {
        Page = pageNumber;
        Size = pageSize;
        Total = total;
    }
    
    public int Page { get; init; }

    public int Size { get; init; }
    public long Total { get; init; } 
}