
using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoTokenDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}