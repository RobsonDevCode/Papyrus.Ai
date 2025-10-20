using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Papyrus.Domain.Services.Interfaces;

namespace Papyrus.Domain.Services.Pdf;

public sealed class PdfReaderService : IPdfReaderService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly IPageReaderService _pageReaderService;
    private readonly IMemoryCache _memoryCache;
    private readonly string _bucketName;

    public PdfReaderService(IConfiguration configuration,
        IAmazonS3 amazonS3,
        IPageReaderService pageReaderService,
        IMemoryCache memoryCache)
    {
        _bucketName = configuration.GetValue<string>("AWS:PdfBucketName")
            ?? throw new ArgumentNullException("Bucket name cannot be null");
        _amazonS3 = amazonS3;
        _pageReaderService = pageReaderService;
        _memoryCache = memoryCache;
    }
    public async Task<byte[]> GetPdfBytesAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        var result = await _memoryCache.GetOrCreate(documentGroupId, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            var page = await _pageReaderService.GetByGroupIdAsync(documentGroupId, userId, null, cancellationToken);
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