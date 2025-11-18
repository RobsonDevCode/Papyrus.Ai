using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class BookmarkReader : IBookmarkReader
{
    private readonly IMongoCollection<Bookmark> _bookmarkCollection;

    public BookmarkReader(IMongoBookDbConnector connector)
    {
        _bookmarkCollection = connector.GetCollection<Bookmark>(DatabaseConstants.BookmarksCollectionName);
    }
    
    public async Task<Bookmark?> GetByGroupIdAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        return await _bookmarkCollection.Find(bookmark => bookmark.DocumentGroupId == documentGroupId 
                                                          && bookmark.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken); 
    }
}