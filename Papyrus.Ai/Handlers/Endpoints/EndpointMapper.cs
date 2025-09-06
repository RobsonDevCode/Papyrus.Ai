using Papyrus.Ai.Handlers.Endpoints.Bookmark;
using Papyrus.Ai.Handlers.Endpoints.Documents;
using Papyrus.Ai.Handlers.Endpoints.Images;
using Papyrus.Ai.Handlers.Endpoints.Notes;

namespace Papyrus.Ai.Handlers.Endpoints;

public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapDocumentEndpoints();
        app.MapBookmarkEndpoints();
        
        app.MapNoteWriterEndpoints();
        app.MapNoteReaderEndpoints();
        app.MapImageReaderEndpoints();
    }
}