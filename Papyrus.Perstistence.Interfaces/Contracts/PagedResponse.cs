namespace Papyrus.Perstistance.Interfaces.Contracts;

public record PagedResponse<T>
{
    public T[] Data { get; init; }
    
    public int TotalPages { get; init; }
}