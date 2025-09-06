using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Papyrus.Domain.Services.Interfaces;

namespace Papyrus.Domain.Services.Pdf;

public sealed class PdfReaderService : IPdfReaderService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly IDocumentReaderService _documentReaderService;
    private readonly IMemoryCache _memoryCache;
    private readonly string _bucketName;

    public PdfReaderService(IConfiguration configuration,
        IAmazonS3 amazonS3,
        IDocumentReaderService documentReaderService,
        IMemoryCache memoryCache)
    {
        _bucketName = configuration.GetValue<string>("AWS:BucketName")
            ?? throw new ArgumentNullException("Bucket name cannot be null");
        _amazonS3 = amazonS3;
        _documentReaderService = documentReaderService;
        _memoryCache = memoryCache;
    }
    public async Task<byte[]> GetPdfBytesAsync(Guid documentGroupId, CancellationToken cancellationToken)
    {
        var result = await _memoryCache.GetOrCreate(documentGroupId, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            var page = await _documentReaderService.GetByGroupIdAsync(documentGroupId, null, cancellationToken);
            if (page is null)
            {
                return [];
            }
            
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = page.S3Key
            };
        
            using var response = await _amazonS3.GetObjectAsync(request, cancellationToken);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        })!;

        return result;
    }
}