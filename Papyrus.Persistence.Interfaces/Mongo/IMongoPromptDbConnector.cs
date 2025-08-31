using MongoDB.Driver;

namespace Papyrus.Persistance.Interfaces.Mongo;

public interface IMongoPromptDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}