using MongoDB.Driver;

namespace Papyrus.Persistance.Interfaces.Mongo;

public interface IMongoAudioSettingsDbConnector
{
    public IMongoCollection<T> GetCollection<T>(in string collectionName);
}