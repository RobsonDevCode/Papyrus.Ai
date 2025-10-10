using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoPromptDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}