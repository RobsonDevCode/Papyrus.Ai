using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioSettingReader
{
    Task<AudioSettings?> GetAsync(Guid id, CancellationToken cancellationToken);
}