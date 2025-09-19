using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces.Images;

public interface IImageReaderService
{
    ValueTask<MemoryStream?> GetById(Guid id, CancellationToken cancellationToken);
}