namespace Papyrus.Domain.Models;

public record DocumentModel
{
    public Stream PdfStream { get; init; }

    public string Name { get; set; }
    
    public long Size { get; init; }
}