using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public class VoiceWriter : IVoiceWriter
{
    private readonly IMongoCollection<Voice> _voiceSettingsCollection;

    public VoiceWriter(IMongoVoiceDbConnector connector)
    {
        _voiceSettingsCollection = connector.GetCollection<Voice>(DatabaseConstants.VoiceCollectionName);
    }
    
    public async Task InsertVoicesAsync(IEnumerable<Voice> voices)
    {
        await _voiceSettingsCollection.InsertManyAsync(voices);
    }
}