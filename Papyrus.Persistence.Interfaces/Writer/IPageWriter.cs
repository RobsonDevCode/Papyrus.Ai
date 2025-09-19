using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Writer;

public interface IPageWriter
{
    Task InsertAsync(Page document, CancellationToken cancellationToken);

    Task InsertManyAsync(IEnumerable<Page> pages, CancellationToken cancellationToken);
    
}