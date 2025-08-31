using MongoDB.Driver;

namespace Papyrus.Persistance.Interfaces.Mongo;

public interface IMongoBookDbConnector
{
    public IMongoCollection<T> GetCollection<T>(in string collectionName);

}