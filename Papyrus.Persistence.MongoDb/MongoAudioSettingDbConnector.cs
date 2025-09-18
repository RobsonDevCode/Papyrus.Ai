using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Mongo;

namespace Papyrus.Persistence.MongoDb;

public sealed class MongoAudioSettingDbConnector : IMongoAudioSettingsDbConnector
{
    private readonly IMongoDatabase _database;

    public MongoAudioSettingDbConnector(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.AudioSettingsDatabase);
    }
    
    public IMongoCollection<T> GetCollection<T>(in string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}