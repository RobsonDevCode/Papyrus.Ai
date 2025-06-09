using Papyrus.Ai.Handlers.Endpoints.Documents;

namespace Papyrus.Ai.Handlers.Endpoints;

public static class EndpointMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDocumentWriterEndpoints();
        app.MapDocumentReaderEndpoints();

        return app;
    }
}