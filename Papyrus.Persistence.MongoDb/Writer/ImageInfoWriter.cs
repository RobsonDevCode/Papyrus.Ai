using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class ImageInfoWriter : IImageInfoWriterService
{
    private readonly IMongoCollection<Image> _imageCollection;

    public ImageInfoWriter(IMongoBookDbConnector connector)
    {
        _imageCollection = connector.GetCollection<Image>(DatabaseConstants.ImagesCollectionName);
    }
    public async Task InsertAsync(Image image, CancellationToken cancellationToken)
    {
        await _imageCollection.InsertOneAsync(image, null ,cancellationToken);
    }
}