using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class DocumentWriter : IDocumentWriter
{
    private readonly IMongoCollection<Page> _pagesCollection;

    public DocumentWriter(IMongoBookDbConnector connector)
    {
        _pagesCollection = connector.ConnectToPages();
    }

    public async Task WriteDocumentAsync(Page page, CancellationToken cancellationToken)
    {
        await _pagesCollection.InsertOneAsync(page, null, cancellationToken);
    }
}