using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioSettingsWriterService
{
    Task Upsert(AudioSettingsRequestModel settingsRequest, CancellationToken cancellationToken);
}