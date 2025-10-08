using Papyrus.Domain.Services.Interfaces.Images;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Images;

public sealed class ImageReaderService : IImageReaderService
{
    private readonly IImageInfoReader _imageInfoReader;
    private readonly IImageReader _imageReader;

    public ImageReaderService(IImageInfoReader imageInfoReader,
        IImageReader imageReader
      )
    {
        _imageInfoReader = imageInfoReader;
        _imageReader = imageReader;
    }

    public async ValueTask<MemoryStream?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var imageInfo = await _imageInfoReader.GetByIdAsync(id, cancellationToken);
        var imageStream = await _imageReader.GetImageAsStreamAsync(imageInfo.S3Key, cancellationToken);
        
        var memoryStream = new MemoryStream();
        await imageStream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream;
    }
}