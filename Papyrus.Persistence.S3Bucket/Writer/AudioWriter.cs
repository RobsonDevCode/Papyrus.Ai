using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.S3Bucket.Writer;

public sealed class AudioWriter : IAudioWriter
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;

    public AudioWriter(IAmazonS3 amazonS3, IConfiguration configuration)
    {
        _amazonS3 = amazonS3;
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
}