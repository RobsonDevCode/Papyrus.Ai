using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Services.Interfaces;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Documents;

internal static class DocumentWriterEndpoints
{
    internal static void MapDocumentWriterEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("{userId}/save", SaveDocumentAsync)
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .RequireRateLimiting(RateLimitPolicyConstants.FixedWindowLimitPolicy);

        app.MapDelete("{userId}/{documentGroupId}", DeleteAsync);
    }

    private static async Task<Results<Ok, BadRequest<string>>> SaveDocumentAsync(
        [FromRoute] Guid userId,
        IFormFile pdfFile,
        [FromServices] IDocumentWriterService documentWriterService,
        [FromServices] IValidator<FormFile> pdfFileValidator,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Saving Document",
            [User] = userId.ToString(),
            [FileLength] = pdfFile.Length
        });

        
        var validator = await pdfFileValidator.ValidateAsync(pdfFile, cancellationToken);
        if (!validator.IsValid)
        {
            var formattedErrors = string.Join(" | ", validator.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(formattedErrors);
        }

        logger.LogInformation("Reading document from {pdf name}", pdfFile.FileName);

        var document = new DocumentModel
        {
            PdfStream = pdfFile.OpenReadStream(),
            Name = pdfFile.FileName, 
            Size = pdfFile.Length
        };

        await documentWriterService.StoreDocumentAsync(userId, document, cancellationToken);

        logger.LogInformation("Document Saved");
        return TypedResults.Ok();
    }

    private static async Task<Ok> DeleteAsync(
        [FromRoute] Guid userId,
        [FromRoute] Guid documentGroupId,
        [FromServices] IDocumentWriterService documentWriterService,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Deleting Document",
            [User] = userId.ToString(),
            [DocumentGroupId] = documentGroupId.ToString(),
        });

        logger.LogInformation("Deleting Document {docId} for user {userId}", documentGroupId, userId);

        await documentWriterService.DeleteByIdAsync(userId, documentGroupId, cancellationToken);
        return TypedResults.Ok();
    }
}
