using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioSettingsWriter
{
    Task UpsertAsync(AudioSettings audioSettings, CancellationToken cancellationToken);
}