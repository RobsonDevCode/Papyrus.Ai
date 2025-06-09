namespace Papyrus.Domain.Models;

public record DocumentModel
{
    public Stream PdfStream { get; init; }

    public string Name { get; init; }
    public int PdfPageNumber { get; init; }
}