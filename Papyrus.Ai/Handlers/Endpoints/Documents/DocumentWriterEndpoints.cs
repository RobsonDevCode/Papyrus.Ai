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
        app.MapPost("save", SaveDocumentAsync)
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data");
    }

    private static async Task<Results<Ok, BadRequest<string>>> SaveDocumentAsync(
        IFormFile pdfFile,
        [FromServices] IDocumentWriterService documentWriterService,
        [FromServices] IValidator<FormFile> pdfFileValidator,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.DocumentLogger);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Saving Document",
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

        await documentWriterService.StoreDocumentAsync(document, cancellationToken);

        logger.LogInformation("Document Saved");
        return TypedResults.Ok();
    }

}
