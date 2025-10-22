using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioSettingReader
{
    Task<AudioSettings?> GetAsync(CancellationToken cancellationToken);
    
    Task<AudioSettings?> GetByDocIdAsync(Guid userId, Guid documentId,CancellationToken cancellationToken);
}