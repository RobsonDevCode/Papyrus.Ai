using MongoDB.Driver;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class BookmarkWriter : IBookmarkWriter
{
    private readonly IMongoCollection<Bookmark> _bookmarkCollection;

    public BookmarkWriter(IMongoBookDbConnector connector)
    {
        _bookmarkCollection = connector.GetCollection<Bookmark>("bookmarks");    
    }
    
    public async Task InsertAsync(Bookmark bookmark, CancellationToken cancellationToken)
    {
        await _bookmarkCollection.InsertOneAsync(bookmark,  cancellationToken: cancellationToken);
    }
}