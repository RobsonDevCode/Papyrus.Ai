
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioBookWriterService
{
    Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(CreateAudioBookRequestModel bookRequest,
        CancellationToken cancellationToken);
}