using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.S3Bucket;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudiobookWriterService : IAudiobookWriterService
{
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly IAudioReader _audioReader;
    private readonly ILogger<AudiobookWriterService> _logger;

    public AudiobookWriterService(
        IAudioClient audioClient,
        IAudioWriter audioWriter,
        IAudioReader audioReader,
        ILogger<AudiobookWriterService> logger)
    {
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _audioReader = audioReader;
        _logger = logger;
    }

    public async Task<Stream> CreateAsync(CreateAudioRequestModel request, CancellationToken cancellationToken)
    {
        var formattedPages = string.Join("-", request.Pages);
        var s3Key = $"{request.DocumentGroupId}-{request.VoiceId}/{formattedPages}";
        var previouslyMadeAudio = await _audioReader.GetAudioAsync(s3Key, cancellationToken);
        if (previouslyMadeAudio != null)
        {
            _logger.LogInformation(
                "Audio previously created for pages {pages} on document {id} with voice id {voiceId}",
                formattedPages, request.DocumentGroupId, request.VoiceId);
            return previouslyMadeAudio;
        }

        var audio = await _audioClient.CreateAudioAsync(request, cancellationToken);

        await _audioWriter.SaveAsync(s3Key, audio, cancellationToken);
        return audio;
    }
}