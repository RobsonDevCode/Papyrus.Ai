using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class ImageInfoReader : IImageReader
{
    private readonly IMongoCollection<Image> _imageCollection;

    public ImageInfoReader(IMongoBookDbConnector connector)
    {
        _imageCollection = connector.GetCollection<Image>(DatabaseConstants.ImagesCollectionName);    
    }
    
    public async Task<Image> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _imageCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken); 
    }
}