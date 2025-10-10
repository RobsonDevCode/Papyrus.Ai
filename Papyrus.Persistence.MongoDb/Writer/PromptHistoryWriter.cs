using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Writer;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class PromptHistoryWriter : IPromptHistoryWriter
{
    private readonly IMongoCollection<Prompt> _promptCollection;
    private readonly ILogger<PromptHistoryWriter> _logger;
    
    public PromptHistoryWriter(IMongoPromptDbConnector connector, ILogger<PromptHistoryWriter> logger)
    {
        _promptCollection = connector.GetCollection<Prompt>(DatabaseConstants.PromptCollectionName);
        _logger = logger;
    }

    public async Task InsertAsync(Prompt prompt, CancellationToken cancellationToken)
    {
        await _promptCollection.InsertOneAsync(prompt, cancellationToken: cancellationToken);
        _logger.LogInformation("Prompt {id} has been saved", prompt.Id);
    }
}