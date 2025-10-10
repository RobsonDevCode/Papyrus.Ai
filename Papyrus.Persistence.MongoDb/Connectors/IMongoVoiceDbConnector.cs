using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoVoiceDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}