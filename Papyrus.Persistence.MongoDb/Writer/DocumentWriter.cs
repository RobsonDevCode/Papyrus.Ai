using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class DocumentWriter : IDocumentWriter
{
    private readonly IMongoCollection<DocumentPreview> _collection;

    public DocumentWriter(IMongoBookDbConnector connector)
    {
        _collection = connector.GetCollection<DocumentPreview>(DatabaseConstants.DocumentsCollectionName);
    }
    
    public Task InsertAsync(DocumentPreview documentPreview, CancellationToken cancellationToken)
    {
        return _collection.InsertOneAsync(documentPreview, cancellationToken: cancellationToken);
    }
}