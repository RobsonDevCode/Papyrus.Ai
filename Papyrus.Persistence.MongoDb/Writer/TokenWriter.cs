using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class TokenWriter : ITokenWriter
{
    private readonly IMongoCollection<RefreshToken> _collection;

    public TokenWriter(IMongoTokenDbConnector connector)
    {
        _collection = connector.GetCollection<RefreshToken>(DatabaseConstants.RefreshTokenCollectionName);
    }
    
    public Task SaveAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        return _collection.InsertOneAsync(refreshToken, null, cancellationToken);
    }

    public Task DeleteAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return _collection.DeleteOneAsync(x => x.Token == refreshToken, cancellationToken);
    }
}