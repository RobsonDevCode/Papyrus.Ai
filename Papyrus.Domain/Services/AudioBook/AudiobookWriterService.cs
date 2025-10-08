using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.S3Bucket;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudiobookWriterService : IAudiobookWriterService
{
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly IAudioReader _audioReader;
    private readonly IMapper _mapper;
    private readonly string _papyrusApiUrl;
    private readonly ILogger<AudiobookWriterService> _logger;

    public AudiobookWriterService(
        IAudioClient audioClient,
        IAudioWriter audioWriter,
        IAudioReader audioReader,
        IMapper mapper,
        IConfiguration config,
        ILogger<AudiobookWriterService> logger)
    {
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _audioReader = audioReader;
        _mapper = mapper;
        _papyrusApiUrl = config.GetValue<string>("PapyrusApiUrl")
                         ?? throw new NullReferenceException("PapyrusApiUrl cannot be null when saving document");
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

    public async Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(CreateAudioRequestModel request,
        CancellationToken cancellationToken)
    {
        var formattedPages = string.Join("-", request.Pages);
        var audioS3 = $"{request.DocumentGroupId}-{request.VoiceId}/{formattedPages}/Audio";
        var alignmentS3Key = $"{request.DocumentGroupId}-{request.VoiceId}/{formattedPages}/alignment";
        
        if (await _audioReader.ExistsAsync(audioS3, cancellationToken))
        {
            _logger.LogInformation(
                "Audio previously created for pages {pages} on document {id} with voice id {voiceId}",
                formattedPages, request.DocumentGroupId, request.VoiceId);
            
            var alignment = await _audioReader.GetAlignmentInformationAsync(alignmentS3Key, cancellationToken);
            if (alignment is null)
            {
                throw new Exception($"{request.DocumentGroupId} on pages {formattedPages} has audio but no alignment exists");
            }
            
            return new AudioAlignmentResultModel
            {
                AudioUrl = $"{_papyrusApiUrl}/text-to-speech/{request.DocumentGroupId}/{request.VoiceId}?pageNumbers={request.Pages[0]}&pageNumbers={request.Pages[1]}",
                Alignment = _mapper.MapToDomain(alignment!).ToList() 
            };
        }

        request = request with
        {
            Text = request.Text.Replace("-", "")
        };

        var audioResult = await _audioClient.CreateWithAlignmentAsync(request, cancellationToken);
        await _audioWriter.SaveAsync(audioS3, audioResult.AudioStream, cancellationToken);
        await _audioWriter.SaveAlignmentsAsync(alignmentS3Key,_mapper.MapToPersistence(audioResult.NormalizedAlignment), cancellationToken);
        return new AudioAlignmentResultModel
        {
            AudioUrl =  $"{_papyrusApiUrl}/text-to-speech/{request.DocumentGroupId}/{request.VoiceId}?pageNumbers={request.Pages[0]}&pageNumbers={request.Pages[1]}",
            Alignment = audioResult.NormalizedAlignment.ToList()
        };
    }
}