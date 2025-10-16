using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoUserDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}