using Microsoft.Extensions.Caching.Memory;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.Images;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Images;

public sealed class ImageReaderService : IImageReaderService
{
    private readonly IImageInfoReader _imageInfoReader;
    private readonly IImageReader _imageReader;
    private readonly IMemoryCache _memoryCache;

    public ImageReaderService(IImageInfoReader imageInfoReader,
        IImageReader imageReader,
        IMemoryCache memoryCache)
    { 
        _imageInfoReader = imageInfoReader;
        _imageReader = imageReader;
        _memoryCache = memoryCache;
    }
    
    public async ValueTask<MemoryStream?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var stream = await _memoryCache.GetOrCreateAsync(id ,async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var imageInfo = await _imageInfoReader.GetByIdAsync(id, cancellationToken);
            var imageStream = await _imageReader.GetImageAsStreamAsync(imageInfo.S3Key, cancellationToken);
            
            return imageStream;
        });

        if (stream == null)
        {
            return null;
        }
        
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream;
    }
}