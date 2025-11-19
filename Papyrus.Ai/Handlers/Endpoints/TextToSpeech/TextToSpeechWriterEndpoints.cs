using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Domain.Services.Interfaces.ExplanationTextToSpeech;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechWriterEndpoints
{
    internal static void MapTextToSpeechWriterEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("", CreateWithAlignment)
            .RequireRateLimiting(RateLimitPolicyConstants.IpPolicy);

        app.MapPost("explanation", CreateExplanationWithAlignment)
            .RequireRateLimiting(RateLimitPolicyConstants.IpPolicy);
        
        app.MapPost("setting", AddSettings);
    }

    private static async Task<Results<Ok<AudioWithAlignmentResponse>, BadRequest<string>>> CreateWithAlignment(
        [FromBody] CreateAudioBookRequest request,
        [FromServices] IValidator<CreateAudioBookRequest> requestValidator,
        [FromServices] IAudioBookWriterService audioBookWriterService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Creating Audio with Alignment",
            [DocumentGroupId] = request.DocumentGroupId,
            [Filter] = request,
            [User] = request.UserId
        });

        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | " + validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        logger.LogInformation("Creating Audio for document {id} with voice {voiceId}", request.DocumentGroupId,
            request.VoiceId);

        var mappedRequest = mapper.MapToDomain(request);
        var audioResult = await audioBookWriterService.CreateWithAlignmentAsync(mappedRequest, cancellationToken);

        return TypedResults.Ok(mapper.MapToResponse(audioResult));
    }

    private static async Task<Results<Ok<AudioWithAlignmentResponse>, BadRequest<string>>> CreateExplanationWithAlignment(
        [FromBody] CreateExplanationTextToSpeechRequest request,
        [FromServices] IValidator<CreateExplanationTextToSpeechRequest> requestValidator,
        [FromServices] IExplanationTextToSpeechService explanationTextToSpeechService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Creating Explanation Text To Speech with Alignment",
            [Filter] = request
        });
        
        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | " + validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }
        
        var userId = context.User.GetUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        
        logger.LogInformation("Creating audio for explanation text: {text}", request.Text);
        var mappedRequest = mapper.MapToDomain(request);
        var audioResults = await explanationTextToSpeechService.CreateWithAlignmentAsync(userId.Value, mappedRequest, cancellationToken);
        
        return TypedResults.Ok(mapper.MapToResponse(audioResults));
    }


    private static async Task<Results<Created, BadRequest<string>>> AddSettings(
        [FromBody] AudioSettingsRequest request,
        [FromServices] IValidator<AudioSettingsRequest> requestValidator,
        [FromServices] IAudioSettingsWriterService audioSettingsWriterService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Set Up Text To Speech Settings",
            [Filter] = request
        });

        logger.LogInformation("Adding or updating Audio Settings");

        var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var domainRequest = mapper.MapToDomain(request);
        await audioSettingsWriterService.Upsert(domainRequest, cancellationToken);

        return TypedResults.Created();
    }
}