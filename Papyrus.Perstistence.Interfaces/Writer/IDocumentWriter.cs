using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IDocumentWriter
{
    Task InsertAsync(Page document, CancellationToken cancellationToken);

    Task InsertManyAsync(IEnumerable<Page> pages, CancellationToken cancellationToken);
}