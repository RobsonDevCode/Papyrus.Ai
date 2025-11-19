using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Services.Interfaces.ExplanationTextToSpeech;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.S3Bucket;

namespace Papyrus.Domain.Services.ExplanationTextToSpeech;

public sealed class ExplanationTextToSpeechService : IExplanationTextToSpeechService
{
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly IAudioReader _audioReader;
    private readonly IMapper _mapper;
    private readonly string _papyrusApiUrl;
    private readonly ILogger<ExplanationTextToSpeechService> _logger;

    public ExplanationTextToSpeechService(IAudioClient audioClient, IAudioWriter audioWriter, IAudioReader audioReader,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<ExplanationTextToSpeechService> logger)
    {
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _audioReader = audioReader;
        _mapper = mapper;
        _papyrusApiUrl = configuration.GetValue<string>("PapyrusApiUrl") ??
                         throw new NullReferenceException(
                             "PapyrusApiUrl cannot be null when creating Explanation Text To Speech");
        _logger = logger;
    }

    public async Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(Guid userId, CreateTextToSpeechRequestModel request,
        CancellationToken cancellationToken)
    {
        var baseKey = $"explanations/{userId}/{request.Id}";
        var audioS3Key = $"{baseKey}/audio";
        var alignmentS3Key = $"{baseKey}/alignment";

        if (await _audioReader.ExistsAsync(audioS3Key, cancellationToken))
        {
            _logger.LogInformation(
                "Audio previously created for explanation {id} with voice id {voiceId}",
                request.Id, request);

            var alignment = await _audioReader.GetAlignmentInformationAsync(alignmentS3Key, cancellationToken);
            if (alignment is null)
            {
                throw new Exception($"Explanation {request.Id} has audio but no alignment exists");
            }

            return new AudioAlignmentResultModel
            {
                AudioUrl = $"{_papyrusApiUrl}/text-to-speech/{request.Id}",
                Alignment = _mapper.MapToDomain(alignment!).ToList()
            };
        }

        var audioResult = await _audioClient.CreateWithAlignmentAsync(request.Text, request.VoiceSettings,
            request.VoiceId, cancellationToken);
        
        await _audioWriter.SaveAsync(audioS3Key, audioResult.AudioStream, cancellationToken);
        await _audioWriter.SaveAlignmentsAsync(alignmentS3Key,
            _mapper.MapToPersistence(audioResult.NormalizedAlignment), cancellationToken);
        return new AudioAlignmentResultModel
        {
            AudioUrl = $"{_papyrusApiUrl}/text-to-speech/{request.Id}",
            Alignment = audioResult.NormalizedAlignment.ToList()
        };
    }
}