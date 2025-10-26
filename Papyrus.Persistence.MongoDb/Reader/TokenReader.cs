using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class TokenReader : ITokenReader
{
    private IMongoCollection<RefreshToken> _collection;

    public TokenReader(IMongoTokenDbConnector connector)
    {
        _collection = connector.GetCollection<RefreshToken>(DatabaseConstants.RefreshTokenCollectionName);
    }

    public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken)
    {
        return await _collection.Find(x => x.Token == token).SingleOrDefaultAsync(cancellationToken);
    }
}