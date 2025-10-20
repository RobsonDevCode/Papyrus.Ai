using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentReaderEndpoints
{
    internal static void MapDocumentReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("page/{documentGroupId}/{pageNumber}", GetPage);

        app.MapGet("pages/{documentGroupId}", GetPages);
        
        app.MapGet("{userId}/{documentGroupId}", GetDocument);
        
        app.MapGet("{userId}", GetDocuments);
    }

    private static async Task<Results<Ok<DocumentPageResponse>, NotFound, BadRequest<string>>> GetPage(
        [FromRoute] Guid documentGroupId,
        [FromRoute] int pageNumber,
        [FromServices] IPageReaderService pageReaderService,
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

        var response = await pageReaderService.GetByGroupIdAsync(documentGroupId, pageNumber, cancellationToken);

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
        [FromServices] IPageReaderService pageReaderService,
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
        
        var response = await pageReaderService.GetPages(documentGroupId, pageNumbers, cancellationToken);
        
        return TypedResults.Ok(mapper.MapToResponse(response.Pages, response.TotalPages));
    }

    private static async Task<Results<FileContentHttpResult, NotFound>> GetDocument(
        [FromRoute] Guid userId,
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

        var response = await pdfReaderService.GetPdfBytesAsync(userId, documentGroupId, cancellationToken);
        if (response.Length == 0)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.File(response, "application/pdf", $"{documentGroupId}.pdf");
    }

    private static async Task<Ok<PagedResponse<DocumentResponse>>> GetDocuments(
        [FromRoute] Guid userId,
        [AsParameters] PaginationOptions paginationOptions,
        [FromQuery] string? searchTerm,
        [FromServices] IDocumentReaderService documentReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Documents",
            [Filter] = paginationOptions,
            [User] = userId.ToString()
        });

        logger.LogInformation("Getting Documents page {page}, page size {pageSize} for user {id}", paginationOptions.Page, paginationOptions.Size, userId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            logger.LogInformation("search term: {searchTerm}", searchTerm);
        }
        
        var response = await documentReaderService.GetDocuments(userId, searchTerm, new PaginationRequestModel
        {
            Page = paginationOptions.Page,
            Size = paginationOptions.Size
        }, cancellationToken);

        var documents = mapper.MapToResponse(response.Items.ToArray());
        var result = mapper.MapToResponse(documents, paginationOptions.Page, paginationOptions.Size, response.Pagination.TotalPages);
        
        return TypedResults.Ok(result);
    }
}