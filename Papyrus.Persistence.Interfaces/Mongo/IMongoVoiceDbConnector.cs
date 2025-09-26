using MongoDB.Driver;

namespace Papyrus.Persistance.Interfaces.Mongo;

public interface IMongoVoiceDbConnector
{
    IMongoCollection<T> GetCollection<T>(in string collectionName);
}