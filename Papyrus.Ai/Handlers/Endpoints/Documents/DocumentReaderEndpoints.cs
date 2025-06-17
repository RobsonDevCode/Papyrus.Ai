using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentReaderEndpoints
{
    internal static void MapDocumentReaderEndpoints(this WebApplication app)
    {
        var documentGroup = app.MapGroup("document");

        documentGroup.MapGet("{documentGroupId}/{pageNumber}", GetPage)
            .WithTags(DocumentApiTags.DocumentReader);
    }

    private static async Task<Results<Ok<DocumentPageResponse>, NotFound, BadRequest<string>>> GetPage(
        [FromRoute] Guid documentGroupId,
        [FromRoute] int pageNumber,
        [FromServices] IDocumentReaderService documentReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentReader);

        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Document Page",
            [Filter] = documentGroupId.ToString()
        });

        var response = await documentReaderService.GetPageByIdAsync(documentGroupId, pageNumber, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("No Page was found");
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.MapToResponse(response));
    }
}