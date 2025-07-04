﻿using Microsoft.Extensions.Caching.Memory;
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

    public async Task<PageModel?> GetByIdAsync(Guid pageId, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(pageId, async entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

            var page = await _documentReader.GetByIdAsync(pageId, cancellationToken);
            return page is null ? null : _mapper.MapToDomain(page);
        });
    } 
    
    public async Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, int? pageNumber,
        CancellationToken cancellationToken)
    {
        if (pageNumber is null or 0)
        {
            pageNumber = 1;
        }

        return await _memoryCache.GetOrCreateAsync($"{documentGroupId}-{pageNumber}", async entry =>
        {
            entry.Size = 100;
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            var page = await _documentReader.GetByGroupIdAsync(documentGroupId, (int)pageNumber, cancellationToken);
            return page is null ? null : _mapper.MapToDomain(page);
        });
    }
}