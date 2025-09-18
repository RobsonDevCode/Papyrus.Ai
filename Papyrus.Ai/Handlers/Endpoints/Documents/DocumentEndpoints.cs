namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentEndpoints
{
    internal static void MapDocumentEndpoints(this WebApplication app)
    {
        var documentGroup = app.MapGroup("Document").WithTags("document");

        documentGroup.MapDocumentReaderEndpoints();
        documentGroup.MapDocumentWriterEndpoints();
    }
}