namespace Papyrus.Domain.Models.Documents;

public record DocumentModel
{
    public Stream PdfStream { get; init; }

    public string Name { get; set; }
    
    public long Size { get; init; }
}