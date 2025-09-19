using Microsoft.Extensions.Logging;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Bookmark;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Domain.Services.Bookmark;

public sealed class BookmarkWriterService : IBookmarkWriterService
{
    private readonly IPageReaderService _pageReaderService;
    private readonly IBookmarkWriter _bookmarkWriter;
    private readonly IMapper _mapper;
    private readonly ILogger<BookmarkWriterService> _logger;

    public BookmarkWriterService(IPageReaderService pageReaderService, IBookmarkWriter bookmarkWriter,
        IMapper mapper,
        ILogger<BookmarkWriterService> logger)
    {
        _pageReaderService = pageReaderService;
        _bookmarkWriter = bookmarkWriter;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task Create(BookmarkModel bookmark, CancellationToken cancellationToken)
    {
        if (await _pageReaderService.GetByGroupIdAsync(bookmark.DocumentGroupId, bookmark.Page, cancellationToken)
            is null)
        {
            _logger.LogError("Document group {id} page {page} not found", bookmark.DocumentGroupId, bookmark.Page);
            throw new KeyNotFoundException($"Document group {bookmark.DocumentGroupId} page {bookmark.Page} not found");
        }

        var mapped = _mapper.MapToPersistence(bookmark);
        await _bookmarkWriter.InsertAsync(mapped, cancellationToken);        
    }
}