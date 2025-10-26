using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistence.MongoDb.Connectors;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Reader;

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

    public async Task<AudioSettings?> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _audioSettingsCollection.Find(x => x.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<AudioSettings?> GetByDocIdAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        return _audioSettingsCollection.Find(x => x.Id == documentGroupId && x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }
}