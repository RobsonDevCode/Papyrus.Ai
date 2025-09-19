using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Persistence.S3Bucket;

public sealed class ImageReader : IImageReader
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;

    public ImageReader(IAmazonS3 amazonS3, IConfiguration configuration)
    {
        _amazonS3 = amazonS3;
        _bucketName = configuration["AWS:ImageBucketName"] ??
                      throw new ArgumentNullException("Bucket name cannot be null.");
    }

    public async Task<Stream> GetImageAsStreamAsync(string s3key, CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = s3key
        };

        var response = await _amazonS3.GetObjectAsync(request, cancellationToken);
        return response.ResponseStream;
    }
}