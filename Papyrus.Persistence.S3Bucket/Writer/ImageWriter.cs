using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.S3Bucket.Writer;

public sealed class ImageWriter : IImageWriter
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;

    public ImageWriter(IConfiguration configuration, IAmazonS3 amazonS3)
    {
        _bucketName = configuration["AWS:ImageBucketName"] ?? throw new ArgumentNullException("Bucket name cannot be null");
        _amazonS3 = amazonS3;
    }
    
    public async Task SaveAsync(string s3Key, Stream stream, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            InputStream = stream,
            Key = s3Key,
            ContentType = "image/png",
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };
        
        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }
}