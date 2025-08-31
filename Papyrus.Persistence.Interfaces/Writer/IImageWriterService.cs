using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IImageWriterService
{
    Task InsertAsync(Image image, CancellationToken cancellationToken);
}