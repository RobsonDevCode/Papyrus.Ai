using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class AudioSettingsWriter : IAudioSettingsWriter
{
    private readonly IMongoCollection<AudioSettings> _audioSettingsCollection;

    public AudioSettingsWriter(IMongoAudioSettingsDbConnector connector)
    {
        _audioSettingsCollection =
            connector.GetCollection<AudioSettings>(DatabaseConstants.AudioSettingsCollectionName);
    }


    public async Task UpsertAsync(AudioSettings audioSettings, CancellationToken cancellationToken)
    {
        var filter = Builders<AudioSettings>.Filter.Eq(x => x.Id, audioSettings.Id);
        var update = Builders<AudioSettings>.Update
            .Set(x => x.VoiceSetting, audioSettings.VoiceSetting)
            .Set(x => x.VoiceId, audioSettings.VoiceId)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(x => x.Id, audioSettings.Id)
            .SetOnInsert(x => x.UserId, audioSettings.UserId)
            .SetOnInsert(x => x.CreatedAt, DateTime.UtcNow);

        var options = new UpdateOptions { IsUpsert = true };

        await _audioSettingsCollection.UpdateOneAsync(filter, update, options, cancellationToken);
    }

    public Task DeleteAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        return _audioSettingsCollection.DeleteOneAsync(x => x.UserId == userId && x.Id == documentGroupId,
            cancellationToken);
    }
}