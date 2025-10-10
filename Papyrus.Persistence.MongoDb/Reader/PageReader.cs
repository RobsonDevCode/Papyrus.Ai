using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class PageReader : IPageReader
{
    private readonly IMongoCollection<Page> _pageCollection;
    private readonly ILogger<PageReader> _logger;

    public PageReader(IMongoBookDbConnector connector, ILogger<PageReader> logger)
    {
        _pageCollection = connector.GetCollection<Page>(DatabaseConstants.PagesCollectionName);
        _logger = logger;
    }

    public async Task<Page?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _pageCollection.Find(p => p.DocumentId == id).SingleOrDefaultAsync(cancellationToken);
    }

    public async IAsyncEnumerable<Page?> GetByGroupIdAsync(Guid documentId, CancellationToken cancellationToken)
    {
        var filter = Builders<Page>.Filter.Eq(x => x.DocumentGroupId, documentId);
        using var cursor = await _pageCollection.Find(filter).ToCursorAsync(cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (var page in cursor.Current)
            {
                yield return page;
            }
        }
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