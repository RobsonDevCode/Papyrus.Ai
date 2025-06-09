using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IDocumentWriter
{
    Task WriteDocumentAsync(Page document, CancellationToken cancellationToken);
}