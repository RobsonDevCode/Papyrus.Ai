using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Persistence.MongoDb.Extensions;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class VoiceReader : IVoiceReader
{
    private readonly IMongoCollection<Voice> _voiceCollection;

    public VoiceReader(IMongoVoiceDbConnector connector)
    {
        _voiceCollection = connector.GetCollection<Voice>(DatabaseConstants.VoiceCollectionName);
    }

    public async Task<(IEnumerable<Voice> Voices, int TotalCount)> GetVoicesAsync(VoiceSearch filter,
        CancellationToken cancellationToken)
    {
        var query = _voiceCollection
            .AsQueryable()
            .WhereIf(!string.IsNullOrWhiteSpace(filter.SearchTerm), x => x.Name.Contains(filter.SearchTerm!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Accent),
                x => x.Labels != null && x.Labels.Accent == filter.Accent)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.UseCase),
                x => x.Labels != null && x.Labels.UseCase == filter.UseCase)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Gender),
                x => x.Labels != null && x.Labels.Gender == filter.Gender);
        
        var count = await query.CountAsync(cancellationToken);

        var voices = await query
            .Skip((filter.Pagination.Page - 1) * filter.Pagination.Size)
            .Take(filter.Pagination.Size)
            .ToListAsync(cancellationToken);
        
        return (voices, count);
    }
}