using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioSettingsWriterService
{
    Task Upsert(AudioSettingsModel settings, CancellationToken cancellationToken);
}