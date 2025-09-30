
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudiobookWriterService
{
    Task<Stream> CreateAsync(CreateAudioRequestModel request, CancellationToken cancellationToken);
}