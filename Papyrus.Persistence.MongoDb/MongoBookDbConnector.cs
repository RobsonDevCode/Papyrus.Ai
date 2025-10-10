using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb;

public class MongoBookDbConnector : IMongoBookDbConnector
{
    private readonly IMongoDatabase _database;

    public MongoBookDbConnector(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.BooksDatabase);
    }

    public IMongoCollection<T> GetCollection<T>(in string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }


}