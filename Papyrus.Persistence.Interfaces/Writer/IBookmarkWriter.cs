using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IBookmarkWriter
{
    Task InsertAsync(Bookmark bookmark, CancellationToken cancellationToken);
}