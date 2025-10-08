using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechReaderEndpoint
{
    internal static void MapTextToSpeechReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("setting", GetSettings);
        app.MapGet("{documentGroupId:guid}/{voiceId}", Get);
    }


    private static async Task<Results<FileStreamHttpResult, NotFound>> Get(
        [FromRoute] Guid documentGroupId,
        [FromRoute] string voiceId,
        [FromQuery] int[] pageNumbers,
        [FromServices] IAudioReaderService audioReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = $"Get Audio {documentGroupId}",
            [DocumentGroupId] = documentGroupId
        });
        
        logger.LogInformation("starting to get audio");
        
        var audioStream = await audioReaderService.GetAsync(documentGroupId, voiceId, pageNumbers, cancellationToken);
        if (audioStream == null)
        {
            return TypedResults.NotFound();
        }

        var memoryStream = new MemoryStream();
        await audioStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        return TypedResults.Stream(memoryStream, "audio/mpeg", "audio-to-speech.mp3", enableRangeProcessing: true);
    }

    private static async Task<Results<Ok<AudioSettingsResponse>, NotFound>> GetSettings(
        [FromServices] IAudioSettingsReaderService audioSettingsReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Getting Audio Settings"
        });

        var audioSettings = await audioSettingsReaderService.GetAsync(cancellationToken);
        if (audioSettings == null)
        {
            logger.LogWarning("No audio settings found.");
            return TypedResults.NotFound();
        }

        var result = mapper.MapToResponse(audioSettings);
        return TypedResults.Ok(result);
    }
}