using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IDocumentWriter
{
    Task InsertAsync(DocumentPreview documentPreview, CancellationToken cancellationToken);
}