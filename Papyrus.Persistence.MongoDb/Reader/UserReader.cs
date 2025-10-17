using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class UserReader : IUserReader
{
    private readonly IMongoCollection<User> _userCollection;

    public UserReader(IMongoUserDbConnector connector)
    {
        _userCollection = connector.GetCollection<User>(DatabaseConstants.UsersCollectionName);
    }

    public async Task<bool> ExistsAsync(string? username, string? email, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            return await _userCollection.Find(x => x.Username == username).AnyAsync(cancellationToken);
        }

        return await _userCollection.Find(x => x.Email == email).AnyAsync(cancellationToken);
    }

    public async Task<User?> GetAsync(string email, CancellationToken cancellationToken)
    {
        return await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}