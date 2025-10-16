using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class UserWriter : IUserWriter
{
    private readonly IMongoCollection<User> _userCollection;

    public UserWriter(IMongoUserDbConnector connector)
    {
        _userCollection = connector.GetCollection<User>(DatabaseConstants.UsersCollectionName);
    }
    public Task InsertAsync(User user, CancellationToken cancellationToken)
    {
        return _userCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }
}