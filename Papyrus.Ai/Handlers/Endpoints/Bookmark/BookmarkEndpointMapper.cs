namespace Papyrus.Ai.Handlers.Endpoints.Bookmark;

internal static class BookmarkEndpointMapper
{
    internal static void MapBookmarkEndpoints(this WebApplication app)
    {
        var bookmarkGroup = app.MapGroup("bookmark")
            .WithTags("bookmark");
        bookmarkGroup.MapBookmarkWriterEndpoints();
        bookmarkGroup.MapBookmarkReaderEndpoints();
    }
}