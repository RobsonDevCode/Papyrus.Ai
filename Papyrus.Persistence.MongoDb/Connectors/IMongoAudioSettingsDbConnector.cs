using MongoDB.Driver;

namespace Papyrus.Persistence.MongoDb.Connectors;

public interface IMongoAudioSettingsDbConnector
{
    public IMongoCollection<T> GetCollection<T>(in string collectionName);
}