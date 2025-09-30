using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Papyrus.Persistence.S3Bucket;

public sealed class AudioReader : IAudioReader
{
    private readonly IAmazonS3 _amazonS3;
    private readonly ILogger<IAudioReader> _logger;
    private readonly string _bucketName;

    public AudioReader(IAmazonS3 amazonS3, ILogger<IAudioReader> logger,IConfiguration config)
    {
        _amazonS3 = amazonS3;
        _logger = logger;
        _bucketName = config["AWS:AudioBucketName"]
                      ?? throw new ArgumentNullException("Bucket name cannot be null.");
    }

    public async Task<Stream?> GetAudioAsync(string s3Key, CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key
        };

        try
        {
            var response = await _amazonS3.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("No audio found for {s3Key} creating audio...", s3Key);
            return null;
        }
    }
}