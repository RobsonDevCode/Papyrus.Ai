namespace Papyrus.Ai.Handlers.Endpoints.Bookmark;

internal static class BookmarkEndpointMapper
{
    internal static void MapBookmarkEndpoints(this WebApplication app)
    {
        var bookmarkGroup = app.MapGroup("Bookmark")
            .WithTags("bookmark")
            .RequireAuthorization();
        
        bookmarkGroup.MapBookmarkWriterEndpoints();
        bookmarkGroup.MapBookmarkReaderEndpoints();
    }
}
