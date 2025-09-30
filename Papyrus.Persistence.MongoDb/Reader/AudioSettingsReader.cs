using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class AudioSettingsReader : IAudioSettingReader
{
    private readonly IMongoCollection<AudioSettings> _audioSettingsCollection;

    public AudioSettingsReader(IMongoAudioSettingsDbConnector connector)
    {
        _audioSettingsCollection = connector.GetCollection<AudioSettings>(DatabaseConstants.AudioSettingsCollectionName);
    }
    
    public async Task<AudioSettings?> GetAsync(CancellationToken cancellationToken)
    {
        return await _audioSettingsCollection.Find(x => true).FirstOrDefaultAsync(cancellationToken);
    }
}