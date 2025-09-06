using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentReaderEndpoints
{
    internal static void MapDocumentReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("page/{documentGroupId}/{pageNumber}", GetPage);

        app.MapGet("pages/{documentGroupId}", GetPages);
        
        app.MapGet("{documentGroupId}", GetDocument);
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

        var response = await documentReaderService.GetByGroupIdAsync(documentGroupId, pageNumber, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("No Page was found");
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(mapper.MapToResponse(response));
    }

    private static async Task<Ok<DocumentPagesResponse>> GetPages(
        [FromRoute] Guid documentGroupId,
        [FromQuery] int[] pageNumbers,
        [FromServices] IDocumentReaderService documentReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentReader);

        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Document Pages",
            [Filter] = documentGroupId.ToString(),
            [PageNumbers] = pageNumbers
        });
        
        var response = await documentReaderService.GetPages(documentGroupId, pageNumbers, cancellationToken);
        
        return TypedResults.Ok(mapper.MapToResponse(response.Pages, response.TotalPages));
    }

    private static async Task<Results<FileContentHttpResult, NotFound>> GetDocument(
        [FromRoute] Guid documentGroupId,
        [FromServices] IPdfReaderService pdfReaderService,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentReader);

        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Document",
            [Filter] = documentGroupId.ToString()
        });

        var response = await pdfReaderService.GetPdfBytesAsync(documentGroupId, cancellationToken);
        if (response.Length == 0)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.File(response, "application/pdf", $"{documentGroupId}.pdf");
    }
    
}