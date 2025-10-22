using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Writer;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class PageWriter : IPageWriter
{
    private readonly IMongoCollection<Page> _pagesCollection;

    public PageWriter(IMongoBookDbConnector connector)
    {
        _pagesCollection = connector.GetCollection<Page>(DatabaseConstants.PagesCollectionName);
    }

    public async Task InsertAsync(Page page, CancellationToken cancellationToken)
    {
        await _pagesCollection.InsertOneAsync(page, null, cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<Page> pages, CancellationToken cancellationToken)
    {
        await _pagesCollection.InsertManyAsync(pages, null, cancellationToken);
    }

    public Task DeleteAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        return _pagesCollection.DeleteOneAsync(x => x.UserId == userId && x.DocumentGroupId == documentGroupId, cancellationToken);
    }
}