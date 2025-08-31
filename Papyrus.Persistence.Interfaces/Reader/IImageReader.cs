using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IImageReader
{
    Task<Image> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}