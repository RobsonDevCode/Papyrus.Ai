﻿using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class DocumentReader : IDocumentReader
{
    private readonly IMongoCollection<Page> _pageCollection;
    private readonly ILogger<DocumentReader> _logger;

    public DocumentReader(IMongoBookDbConnector connector, ILogger<DocumentReader> logger)
    {
        _pageCollection = connector.GetCollection<Page>("pages");
        _logger = logger;
    }

    public async Task<Page?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _pageCollection.Find(p => p.DocumentId == id).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Page?> GetByGroupIdAsync(Guid documentGroupId, int page, CancellationToken cancellationToken)
    {
        if (page == 0)
        {
            _logger.LogWarning("GetPageById called with invalid page number 0 returning page 1");
            page = 1;
        }

        var result = await _pageCollection.Find(p => p.DocumentGroupId == documentGroupId && p.PageNumber == page)
                                 .SingleOrDefaultAsync(cancellationToken);
        
        return result;
    }

    public async Task<bool> ExistsAsync(string documentName, CancellationToken cancellationToken)
    {
        var result = await _pageCollection.Find(p => p.DocumentName == documentName).AnyAsync(cancellationToken);
        return result;
    }
    
}