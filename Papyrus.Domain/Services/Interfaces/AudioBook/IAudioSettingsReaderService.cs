using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioSettingsReaderService
{
    Task<AudioSettingsModel?> GetAsync(CancellationToken cancellationToken);
    
    Task<AudioSettingsModel?> GetAsync(Guid userId, CancellationToken cancellationToken); 
}