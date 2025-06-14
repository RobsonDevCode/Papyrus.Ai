using Papyrus.Ai.Handlers.Endpoints.Documents;
using Papyrus.Ai.Handlers.Endpoints.Notes;

namespace Papyrus.Ai.Handlers.Endpoints;

public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapDocumentWriterEndpoints();
        app.MapDocumentReaderEndpoints();
        app.MapNoteWriterEndpoints();
        app.MapNoteReaderEndpoints();
    }
}