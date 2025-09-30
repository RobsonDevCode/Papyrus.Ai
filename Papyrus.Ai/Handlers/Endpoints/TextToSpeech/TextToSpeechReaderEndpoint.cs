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
        app.MapGet("setting", Get);
    }


    private static async Task<Results<Ok<AudioSettingsResponse>, NotFound>> Get(
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