using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IImageInfoWriterService
{
    Task InsertAsync(Image image, CancellationToken cancellationToken);
}