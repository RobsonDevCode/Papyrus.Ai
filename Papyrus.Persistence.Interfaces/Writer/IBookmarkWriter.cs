using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IBookmarkWriter
{
    Task UpsertAsync(Bookmark bookmark, CancellationToken cancellationToken);
    Task DeleteAsync(Guid userId, Guid documentId, CancellationToken cancellationToken);
}