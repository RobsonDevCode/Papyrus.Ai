using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechReaderEndpoint
{
    internal static void MapTextToSpeechReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("setting/{userId}", GetSettings);
        app.MapGet("{userId}/{documentGroupId:guid}/{voiceId}", Get)
            .RequireRateLimiting(RateLimitPolicyConstants.IpPolicy);
        
        app.MapGet("{explanationId}", GetExplanation)
            .RequireRateLimiting(RateLimitPolicyConstants.IpPolicy);
    }
    
    private static async Task<Results<FileStreamHttpResult, NotFound>> Get(
        [FromRoute] Guid userId,
        [FromRoute] Guid documentGroupId,
        [FromRoute] string voiceId,
        [FromQuery] int[] pageNumbers,
        [FromServices] IAudioReaderService audioReaderService,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = $"Get Audio {documentGroupId}",
            [DocumentGroupId] = documentGroupId
        });

        logger.LogInformation("Getting Audio for {userId}, on document {documentGroupId}", userId, documentGroupId);

        var audioStream =
            await audioReaderService.GetAsync(userId, documentGroupId, voiceId, pageNumbers, cancellationToken);
        if (audioStream == null)
        {
            return TypedResults.NotFound();
        }

        var memoryStream = new MemoryStream();
        await audioStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        return TypedResults.Stream(memoryStream, "audio/mpeg",
            "audio-to-speech.mp3", enableRangeProcessing: true);
    }

    private static async Task<Results<FileStreamHttpResult, NotFound>> GetExplanation(
        [FromRoute] Guid explanationId,
        [FromServices] IAudioReaderService audioReaderService,
        [FromServices] ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = $"Get Audio Explanation {explanationId}",
        });
        
        logger.LogInformation("Getting Audio Explanation {id}", explanationId);

        var userId = context.User.GetUserId();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("user not authorized to access this audio");
        }
        
        var audioStream = await audioReaderService.GetAsync(userId.Value, explanationId, cancellationToken);
        if (audioStream == null)
        {
            return TypedResults.NotFound();
        }
        
        var ms = new MemoryStream();
        await audioStream.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;
        
        return TypedResults.Stream(ms, "audio/mpeg", 
            "audio-to-speech.mp3", enableRangeProcessing: true);
    }
    private static async Task<Results<Ok<AudioSettingsResponse>, NotFound>> GetSettings(
        Guid userId,
        [FromServices] IAudioSettingsReaderService audioSettingsReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Getting Audio Settings",
            [User] = userId
        });

        var audioSettings = await audioSettingsReaderService.GetAsync(userId, cancellationToken);
        if (audioSettings == null)
        {
            logger.LogWarning("No audio settings found.");
            return TypedResults.NotFound();
        }

        var result = mapper.MapToResponse(audioSettings);
        return TypedResults.Ok(result);
    }
}