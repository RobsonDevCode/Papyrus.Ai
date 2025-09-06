using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudiobookWriterService : IAudiobookWriterService
{
    private readonly IDocumentReader _documentReader;
    private readonly IAudioClient _audioClient;
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;
    private readonly ILogger<AudiobookWriterService> _logger;

    public AudiobookWriterService(IDocumentReader documentReader, 
        IAudioClient audioClient,
        IConfiguration configuration, 
        IAmazonS3 amazonS3, 
        ILogger<AudiobookWriterService> logger)
    {
        _documentReader = documentReader;
        _audioClient = audioClient;
        _bucketName = configuration.GetValue<string>("AWS:BucketName")
                      ?? throw new ArgumentNullException("Bucket name cannot be null");
        _amazonS3 = amazonS3;
        _logger = logger;
    }

    public Task<byte[]> CreateAsync(Guid documentGroupId, string voiceId, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}