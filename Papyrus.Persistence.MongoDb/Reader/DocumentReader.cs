using Microsoft.Extensions.Logging;
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

    public async Task<Page?> GetPageById(Guid documentGroupId, int page, CancellationToken cancellationToken)
    {
        if (page == 0)
        {
            _logger.LogWarning("GetPageById called with invalid page number 0 returning page 1");
            page = 1;
        }

        var result = await _pageCollection.FindAsync(p => p.DocumentGroupId == documentGroupId
                                                          && p.PageNumber == page,
            cancellationToken: cancellationToken);
        return result.FirstOrDefault();
    }

    public async Task<string?> GetNameById(Guid documentGroupId, CancellationToken cancellationToken)
    {
        return (await _pageCollection.FindAsync(p => p.DocumentGroupId == documentGroupId,
                cancellationToken: cancellationToken)).FirstOrDefault().DocumentName;
    }
}