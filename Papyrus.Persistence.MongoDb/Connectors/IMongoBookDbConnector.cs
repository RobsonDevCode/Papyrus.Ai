using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoBookDbConnector
{
    public IMongoCollection<T> GetCollection<T>(in string collectionName);

}