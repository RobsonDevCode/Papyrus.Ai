using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Mongo;

namespace Papyrus.Persistence.MongoDb;

public class MongoPromptDbConnector : IMongoPromptDbConnector
{
    private readonly IMongoDatabase _database;

    public MongoPromptDbConnector(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.PromptsDatabase);
    }
    
    public IMongoCollection<T> GetCollection<T>(in string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}