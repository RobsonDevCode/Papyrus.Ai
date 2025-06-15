namespace Papyrus.Domain.Models;

public record ImageModel
{
    public required string Bytes { get; init; }
    public float Width { get; init; }
    public float Height { get; init; }
    public int PageReference { get; init; }
    public double X { get; init; }
    public double Y { get; init; }
}