using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;

namespace Papyrus.Persistence.MongoDb;

public class MongoBookDbConnector : IMongoBookDbConnector
{
    private readonly IMongoDatabase _database;
    private const string PagesCollectionName = "pages";

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

    public IMongoCollection<Page> ConnectToPages()
    {
        return _database.GetCollection<Page>(PagesCollectionName);
    }
}