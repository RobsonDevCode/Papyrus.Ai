using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class AudioSettingsWriter : IAudioSettingsWriter
{
    private readonly IMongoCollection<AudioSettings> _audioSettingsCollection;

    public AudioSettingsWriter(IMongoAudioSettingsDbConnector connector)
    {
        _audioSettingsCollection = connector.GetCollection<AudioSettings>(DatabaseConstants.AudioSettingsCollectionName);
    }


    public async Task UpsertAsync(AudioSettings audioSettings, CancellationToken cancellationToken)
    {
        var filter = Builders<AudioSettings>.Filter.Eq(x => x.Id, audioSettings.Id);
        var update = Builders<AudioSettings>.Update
            .Set(x => x.VoiceSetting, audioSettings.VoiceSetting)
            .Set(x => x.VoiceId, audioSettings.VoiceId)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(x => x.Id, audioSettings.Id)
            .SetOnInsert(x => x.CreatedAt, DateTime.UtcNow);
        
        var options = new UpdateOptions { IsUpsert = true };
        
        await _audioSettingsCollection.UpdateOneAsync(filter, update, options, cancellationToken);
    }
}