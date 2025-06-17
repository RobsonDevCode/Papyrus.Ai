using Microsoft.Extensions.Caching.Memory;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Domain.Services;

public sealed class DocumentReaderService : IDocumentReaderService
{
    private readonly IDocumentReader _documentReader;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public DocumentReaderService(IDocumentReader documentReader, IMemoryCache memoryCache, IMapper mapper
    )
    {
        _documentReader = documentReader;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task<PageModel?> GetPageByIdAsync(Guid documentGroupId, int? page,
        CancellationToken cancellationToken)
    {
        if (page is null or 0)
        {
            page = 1;
        }

        return await _memoryCache.GetOrCreateAsync($"{documentGroupId}-{page}", async entry =>
        {
            entry.Size = 100;
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            var response = await _documentReader.GetPageById(documentGroupId, (int)page, cancellationToken);
            return response is null ? null : _mapper.MapToDomain(response);
        });
    }
}