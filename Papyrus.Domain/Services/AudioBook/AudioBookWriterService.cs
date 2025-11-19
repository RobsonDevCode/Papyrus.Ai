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

public sealed class AudioBookWriterService : IAudioBookWriterService
{
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly IAudioReader _audioReader;
    private readonly IMapper _mapper;
    private readonly string _papyrusApiUrl;
    private readonly ILogger<AudioBookWriterService> _logger;

    public AudioBookWriterService(
        IAudioClient audioClient,
        IAudioWriter audioWriter,
        IAudioReader audioReader,
        IMapper mapper,
        IConfiguration config,
        ILogger<AudioBookWriterService> logger)
    {
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _audioReader = audioReader;
        _mapper = mapper;
        _papyrusApiUrl = config.GetValue<string>("PapyrusApiUrl")
                         ?? throw new NullReferenceException("PapyrusApiUrl cannot be null when creating audiobook");
        _logger = logger;
    }

    public async Task<AudioAlignmentResultModel> CreateWithAlignmentAsync(CreateAudioBookRequestModel bookRequest,
        CancellationToken cancellationToken)
    {
        var formattedPages = string.Join("-", bookRequest.Pages);
        var baseKey = $"{bookRequest.UserId}/{bookRequest.DocumentGroupId}-{bookRequest.VoiceId}/{formattedPages}";
        var audioS3 = $"{baseKey}/Audio";
        var alignmentS3Key = $"{baseKey}/alignment";

        if (await _audioReader.ExistsAsync(audioS3, cancellationToken))
        {
            _logger.LogInformation(
                "Audio previously created for pages {pages} on document {id} with voice id {voiceId}",
                formattedPages, bookRequest.DocumentGroupId, bookRequest.VoiceId);

            var alignment = await _audioReader.GetAlignmentInformationAsync(alignmentS3Key, cancellationToken);
            if (alignment is null)
            {
                throw new Exception(
                    $"{bookRequest.DocumentGroupId} on pages {formattedPages} has audio but no alignment exists");
            }

            return new AudioAlignmentResultModel
            {
                AudioUrl =
                    $"{_papyrusApiUrl}/text-to-speech/{bookRequest.UserId}/{bookRequest.DocumentGroupId}/{bookRequest.VoiceId}?pageNumbers={bookRequest.Pages[0]}&pageNumbers={bookRequest.Pages[1]}",
                Alignment = _mapper.MapToDomain(alignment!).ToList()
            };
        }

        var audioResult = await _audioClient.CreateWithAlignmentAsync(bookRequest.Text, bookRequest.VoiceSettings,
            bookRequest.VoiceId, cancellationToken);
        
        await _audioWriter.SaveAsync(audioS3, audioResult.AudioStream, cancellationToken);
        await _audioWriter.SaveAlignmentsAsync(alignmentS3Key,
            _mapper.MapToPersistence(audioResult.NormalizedAlignment), cancellationToken);
        return new AudioAlignmentResultModel
        {
            AudioUrl =
                $"{_papyrusApiUrl}/text-to-speech/{bookRequest.UserId}/{bookRequest.DocumentGroupId}/{bookRequest.VoiceId}?pageNumbers={bookRequest.Pages[0]}&pageNumbers={bookRequest.Pages[1]}",
            Alignment = audioResult.NormalizedAlignment.ToList()
        };
    }
}