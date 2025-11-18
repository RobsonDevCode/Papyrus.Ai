using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces.Bookmark;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Bookmark;

public sealed class BookmarkReaderService : IBookmarkReaderService
{
    private readonly IBookmarkReader _bookmarkReader;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<BookmarkReaderService> _logger;

    public BookmarkReaderService(IBookmarkReader bookmarkReader, IMemoryCache cache, IMapper mapper,
        ILogger<BookmarkReaderService> logger)
    {
        _bookmarkReader = bookmarkReader;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<BookmarkModel?> GetByGroupIdAsync(Guid userId, Guid documentGroupId,
        CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync(key: $"{userId}-{documentGroupId}", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            var bookmark = await _bookmarkReader.GetByGroupIdAsync(userId, documentGroupId, cancellationToken);
            if (bookmark == null)
            {
                _logger.LogInformation("No bookmark found for group {documentGroupId}", documentGroupId);
                return null;
            }

            return _mapper.MapToDomain(bookmark);
        });
    }
}