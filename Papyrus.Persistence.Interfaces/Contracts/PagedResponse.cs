namespace Papyrus.Persistance.Interfaces.Contracts;

public record PagedResponse<T>
{
    public List<T> Data { get; init; } = [];
    
    public long TotalPages { get; init; }
    
}