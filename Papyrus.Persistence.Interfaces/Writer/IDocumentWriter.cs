using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IDocumentWriter
{
    Task InsertAsync(DocumentPreview documentPreview, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid userId, Guid documentId, CancellationToken cancellationToken);
}