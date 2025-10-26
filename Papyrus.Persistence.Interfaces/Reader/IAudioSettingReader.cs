using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface IAudioSettingReader
{
    Task<AudioSettings?> GetAsync(CancellationToken cancellationToken);
    
    Task<AudioSettings?> GetAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<AudioSettings?> GetByDocIdAsync(Guid userId, Guid documentId,CancellationToken cancellationToken);
}