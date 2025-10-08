
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudiobookWriterService
{
    Task<Stream> CreateAsync(CreateAudioRequestModel request, CancellationToken cancellationToken);

    Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(CreateAudioRequestModel request,
        CancellationToken cancellationToken);
}