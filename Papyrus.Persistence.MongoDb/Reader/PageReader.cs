using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Persistence.MongoDb.Extensions;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;

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

    public async Task<Page?> GetAsync(PageReaderFilters filters, CancellationToken cancellationToken)
    {
        return await _pageCollection.AsQueryable()
            .WhereIf(filters.DocumentGroupId.HasValue, p => p.DocumentGroupId == filters.DocumentGroupId!.Value)
            .WhereIf(filters.PageNumber.HasValue, p => p.PageNumber == filters.PageNumber!.Value)
            .WhereIf(filters.UserId.HasValue, p => p.UserId == filters.UserId!.Value)
            .WhereIf(filters.Id.HasValue, p => p.DocumentId == filters.Id!.Value)
            .SingleOrDefaultAsync(cancellationToken);
    }


    public async Task<Page?> GetByGroupIdAsync(Guid documentGroupId, Guid userId, int page,
        CancellationToken cancellationToken)
    {
        if (page == 0)
        {
            _logger.LogWarning("GetPageById called with invalid page number 0 returning page 1");
            page = 1;
        }

        var result = await _pageCollection.Find(p => p.DocumentGroupId == documentGroupId
                                                     && p.PageNumber == page
                                                     && p.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<bool> ExistsAsync(string documentName,Guid userId, CancellationToken cancellationToken)
    {
        var result = await _pageCollection.Find(p => p.DocumentName == documentName
        && p.UserId == userId).AnyAsync(cancellationToken);
        return result;
    }

    public async Task<(IEnumerable<Page?> Pages, int TotalPages)> GetPages(Guid documentGroupId, Guid userId,
        int[] pageNumbers, CancellationToken cancellationToken)
    {
        var pages = await _pageCollection.Find(x => x.DocumentGroupId == documentGroupId
                                                    && pageNumbers.Contains(x.PageNumber)
                                                    && x.UserId == userId)
            .ToListAsync(cancellationToken);

        var totalPages = await _pageCollection.Find(x => x.DocumentGroupId == documentGroupId
                                                         && x.UserId == userId)
            .CountDocumentsAsync(cancellationToken);

        return (pages, (int)totalPages);
    }
}