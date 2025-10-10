using MongoDB.Driver;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class PromptHistoryReader : IPromptHistoryReader
{
    private readonly IMongoCollection<Prompt> _promptCollection;

    public PromptHistoryReader(IMongoPromptDbConnector connector)
    {
        _promptCollection = connector.GetCollection<Prompt>("prompts");
    }

    public async Task<List<Prompt>> GetHistory(Guid noteId, CancellationToken cancellationToken)
    {
        return await _promptCollection.Find(x => x.NoteId == noteId)
            .Sort(Builders<Prompt>.Sort.Ascending(x => x.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
