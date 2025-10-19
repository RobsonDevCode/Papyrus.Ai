using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class BookmarkWriter : IBookmarkWriter
{
    private readonly IMongoCollection<Bookmark> _bookmarkCollection;

    public BookmarkWriter(IMongoBookDbConnector connector)
    {
        _bookmarkCollection = connector.GetCollection<Bookmark>(DatabaseConstants.BookmarksCollectionName);    
    }
    
    public async Task UpsertAsync(Bookmark bookmark, CancellationToken cancellationToken)
    {
        var filter = Builders<Bookmark>.Filter.Eq(x => x.Id, bookmark.Id);
        var update = Builders<Bookmark>.Update
            .Set(x => x.DocumentGroupId, bookmark.DocumentGroupId)
            .Set(x => x.Page, bookmark.Page)
            .Set(x => x.UserId, bookmark.UserId)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(x => x.Id, bookmark.Id)
            .SetOnInsert(x => x.CreatedAt, bookmark.CreatedAt);
        
        var options = new UpdateOptions { IsUpsert = true };
        await _bookmarkCollection.UpdateOneAsync(filter, update, options, cancellationToken);
    }
}