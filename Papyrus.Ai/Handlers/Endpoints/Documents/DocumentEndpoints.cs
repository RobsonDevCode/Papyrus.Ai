namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentEndpoints
{
    internal static void MapDocumentEndpoints(this WebApplication app)
    {
        var documentGroup = app.MapGroup("document")
            .WithTags("Document")
            .RequireAuthorization();

        documentGroup.MapDocumentReaderEndpoints();
        documentGroup.MapDocumentWriterEndpoints();
    }
}