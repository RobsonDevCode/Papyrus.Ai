using System.Net;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.S3Bucket.Writer;

public sealed class AudioWriter : IAudioWriter
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;
    private readonly ILogger<AudioWriter> _logger;
    
    public AudioWriter(IAmazonS3 amazonS3, 
        ILogger<AudioWriter> logger,
        IConfiguration configuration)
    {
        _amazonS3 = amazonS3;
        _logger = logger;
        _bucketName = configuration["AWS:AudioBucketName"] 
                      ?? throw new ArgumentNullException("Bucket name cannot be null.");
    }
    public async Task SaveAsync(string s3Key, Stream audioStream, CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();
        await audioStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
    
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            InputStream = memoryStream,
            Key = s3Key,
            ContentType = "audio/mpeg",
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };
    
        await _amazonS3.PutObjectAsync(request, cancellationToken);
    
        audioStream.Position = 0;
    }


    public async Task SaveAlignmentsAsync(string s3Key, IEnumerable<AlignmentData> alignment, CancellationToken cancellationToken)
    {
        var jsonContent = JsonSerializer.Serialize(alignment);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key,
            ContentBody = jsonContent,
            ContentType = "application/json"
        };
        
        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }

    public async Task DeleteAsync(string s3Key, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key
        };

        try
        {
            await _amazonS3.DeleteObjectAsync(request, cancellationToken);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("No audio found for {s3Key}", s3Key);
        }
    }
}