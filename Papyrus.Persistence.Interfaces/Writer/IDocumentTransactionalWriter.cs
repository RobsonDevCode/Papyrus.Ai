namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IDocumentTransactionalWriter
{
    Task DeleteDocumentTransaction(Guid userId, Guid documentGroupId, CancellationToken cancellationToken);
}