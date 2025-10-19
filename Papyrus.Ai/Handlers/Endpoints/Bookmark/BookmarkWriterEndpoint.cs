using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.Bookmark;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Bookmark;

internal static class BookmarkWriterEndpoint
{
    internal static void MapBookmarkWriterEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("", Upsert);
    }

    private static async Task<Results<Created, BadRequest<string>>> Upsert(
        [FromBody] CreateBookmarkRequest createBookmarkRequest,
        [FromServices] IValidator<CreateBookmarkRequest> validator,
        [FromServices] IBookmarkWriterService bookmarkWriterService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken
        )
    {
        var logger = loggerFactory.CreateLogger(Loggers.BookmarkWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Upsert Bookmark",
            [Filter] = createBookmarkRequest
        });

        var validationResult = await validator.ValidateAsync(createBookmarkRequest, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }
        
        var mappedToDomain = mapper.MapToDomain(createBookmarkRequest);
        await bookmarkWriterService.UpsertAsync(mappedToDomain, cancellationToken);
        
        return TypedResults.Created();
    }
}