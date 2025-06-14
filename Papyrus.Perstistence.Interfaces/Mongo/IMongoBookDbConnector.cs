using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Mongo;

public interface IMongoBookDbConnector
{
    public IMongoCollection<T> GetCollection<T>(in string collectionName);

    public IMongoCollection<Page> ConnectToPages();
}