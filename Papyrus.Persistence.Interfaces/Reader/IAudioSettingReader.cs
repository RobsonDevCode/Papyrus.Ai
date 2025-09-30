using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioSettingReader
{
    Task<AudioSettings?> GetAsync(CancellationToken cancellationToken);
}