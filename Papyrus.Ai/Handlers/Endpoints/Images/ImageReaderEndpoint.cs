using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Domain.Services.Interfaces.Images;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Images;

internal static class ImageReaderEndpoint
{
    internal static void MapImageReaderEndpoints(this WebApplication app)
    {
        var imageGroup = app.MapGroup("image");

        imageGroup.MapGet("{id}", GetImage)
            .WithTags(ImageApiTags.ImageReader)
            .RequireAuthorization();
    }

    private static async Task<Results<FileContentHttpResult, NotFound>> GetImage(
    [FromRoute]Guid id,
    [FromServices] IImageReaderService imageReaderService, 
    [FromServices] ILoggerFactory loggerFactory,
    CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.ImageReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = nameof(GetImage),
            [Filter] = id.ToString()
        });
        
        logger.LogInformation("Getting image {imageId}...", id);
        await using var imageBytes = await imageReaderService.GetById(id, cancellationToken);
        if (imageBytes == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.File(imageBytes.ToArray(), "image/png", 
            "downloaded-image.png");
    }
}