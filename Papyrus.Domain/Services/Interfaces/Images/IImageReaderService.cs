using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces.Images;

public interface IImageReaderService
{
    ValueTask<ImageModel?> GetById(Guid id, CancellationToken cancellationToken);
}