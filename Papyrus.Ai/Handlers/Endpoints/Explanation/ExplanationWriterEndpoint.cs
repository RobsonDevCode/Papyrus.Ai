using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Explanation;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Explanation;

internal static class ExplanationWriterEndpoint
{
    internal static void MapToExplanationWriterEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("", CreatingExplanation);
    }

    private static async Task<Results<Ok<ExplanationResponse>, BadRequest<string>>> CreatingExplanation(
        [FromBody] CreateExplanationRequest request,
        [FromServices] IExplanationReaderService explanationReaderService,
        [FromServices] IValidator<CreateExplanationRequest> requestValidator,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.ExplanationReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Creating Explanation",
            [Filter] = request
        });

        logger.LogInformation("Getting Explanation for {text} on page number:{page} documentGroupId: {documentGroupId}",
            request.TextToExplain, request.PageNumber, request.DocumentGroupId);

        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var userId = user.GetUserId();
        if (userId is null)
        {
            logger.LogError("users id was not found in claims");
            throw new UnauthorizedAccessException();
        }

        var response =
            await explanationReaderService.GetAsync(mapper.MapToDomain(request, userId.Value), cancellationToken);
        return TypedResults.Ok(new ExplanationResponse(Guid.NewGuid(), response));
    }
}