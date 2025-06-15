namespace Papyrus.Perstistance.Interfaces.Contracts;

public record PaginationOptions
{
    public int Page { get; init; } = 1;
    
    public int Size { get; init; } = 100;
    
    public int? PdfPage { get; init; } = null;
}