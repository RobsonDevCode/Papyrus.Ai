using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.Bookmark;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Bookmark;

internal static class BookmarkReaderEndpointMapper
{
    internal static void MapBookmarkReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("{userId}/{documentGroupId:guid}", GetBookmark);
    }

    private static async Task<Results<Ok<BookmarkResponse>, NotFound>> GetBookmark(
        [FromRoute] Guid userId,
        [FromRoute] Guid documentGroupId,
        [FromServices] IBookmarkReaderService bookmarkReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.BookmarkWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Bookmark",
            [DocumentGroupId] = documentGroupId 
        });

        logger.LogInformation("Getting Bookmark for user {userId}, for document {id}", userId ,documentGroupId);
        var bookmark = await bookmarkReaderService.GetByGroupIdAsync(userId, documentGroupId, cancellationToken);
        if (bookmark == null)
        {
            logger.LogInformation("No bookmark for {id}", documentGroupId);
            return TypedResults.NotFound();
        }
        
        var result = mapper.MapToResponse(bookmark);
        return TypedResults.Ok(result);
    }
}