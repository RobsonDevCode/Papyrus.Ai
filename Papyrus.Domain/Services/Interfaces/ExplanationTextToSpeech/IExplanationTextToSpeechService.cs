using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Services.Interfaces.ExplanationTextToSpeech;

public interface IExplanationTextToSpeechService
{
    Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(Guid userId, CreateTextToSpeechRequestModel request, CancellationToken cancellationToken);
}