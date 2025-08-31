using Microsoft.Extensions.Caching.Memory;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces.Images;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Images;

public sealed class ImageReaderService : IImageReaderService
{
    private readonly IImageReader _imageReader;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public ImageReaderService(IImageReader imageReader,
        IMemoryCache memoryCache,
        IMapper mapper)
    { 
        _imageReader = imageReader;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }
    
    public async ValueTask<ImageModel?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _memoryCache.GetOrCreateAsync(id ,async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var response = await _imageReader.GetByIdAsync(id, cancellationToken);
            
            var result = _mapper.MapToDomain(response);
            return result;
        });
        
        return result;
    }
}