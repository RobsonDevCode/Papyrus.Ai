using Microsoft.Extensions.Logging;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistence.S3Bucket;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudioReaderService : IAudioReaderService
{
    private readonly IAudioReader _audioReader;
    private readonly ILogger<AudioReaderService> _logger;

    public AudioReaderService(IAudioReader audioReader, ILogger<AudioReaderService> logger)
    {
        _audioReader = audioReader;
        _logger = logger;
    }

    public async Task<Stream?> GetAsync(Guid documentGroupId, string voiceId, int[] pageNumbers,
        CancellationToken cancellationToken)
    {
        var formattedPages = string.Join('-', pageNumbers);
        var s3Key = $"{documentGroupId}-{voiceId}/{formattedPages}/Audio";
        
        _logger.LogInformation("Getting Audio for pages {pages}", formattedPages);
        var result = await _audioReader.GetAudioAsync(s3Key, cancellationToken);
        return result;
    }
}