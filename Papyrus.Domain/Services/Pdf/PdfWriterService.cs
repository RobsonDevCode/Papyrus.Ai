using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Papyrus.Domain.Services.Interfaces;

namespace Papyrus.Domain.Services.Pdf;

public class PdfWriterService : IPdfWriterService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName;
    
    public PdfWriterService(IConfiguration configuration, IAmazonS3 amazonS3)
    {
        _bucketName = configuration.GetValue<string>("AWS:PdfBucketName")
            ?? throw new ArgumentNullException("Bucket name cannot be null");
        _amazonS3 = amazonS3;
    }

    public async Task SaveAsync(string s3Key, Stream pdfStream, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            InputStream = pdfStream,
            Key = s3Key,
            ContentType = "application/pdf",
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };
        
        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }
}