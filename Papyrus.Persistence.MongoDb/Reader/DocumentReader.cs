using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class DocumentReader : IDocumentReader
{
    private readonly IMongoCollection<Page> _pageCollection;
    private readonly ILogger<DocumentReader> _logger;

    public DocumentReader(IMongoBookDbConnector connector, ILogger<DocumentReader> logger)
    {
        _pageCollection = connector.GetCollection<Page>(DatabaseConstants.PagesCollectionName);
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

    public async Task<(IEnumerable<Page?> Pages, int TotalPages)> GetPages(Guid documentGroupId, int[] pageNumbers, CancellationToken cancellationToken)
    {
        var pages = await _pageCollection.Find(x => x.DocumentGroupId == documentGroupId
                                               && pageNumbers.Contains(x.PageNumber))
                                               .ToListAsync(cancellationToken);
        
        var totalPages = await _pageCollection.Find(x => x.DocumentGroupId == documentGroupId).CountDocumentsAsync(cancellationToken);
        
        return (pages, (int)totalPages);
    }
}