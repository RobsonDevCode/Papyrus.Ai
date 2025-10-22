using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioSettingsWriter
{
    Task UpsertAsync(AudioSettings audioSettings, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken);
}