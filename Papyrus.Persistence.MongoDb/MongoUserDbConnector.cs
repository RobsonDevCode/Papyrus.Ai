using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb;

public sealed class MongoUserDbConnector : IMongoUserDbConnector
{
    private readonly IMongoDatabase _database;

    public MongoUserDbConnector(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.UserDatabase);
    }

    public IMongoCollection<T> GetCollection<T>(in string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}