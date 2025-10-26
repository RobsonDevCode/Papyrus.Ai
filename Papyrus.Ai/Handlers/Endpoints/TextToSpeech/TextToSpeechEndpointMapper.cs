namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechEndpointMapper
{
    internal static void MapTextToSpeechEndpoints(this WebApplication app)
    {
        var textToSpeechGroup = app.MapGroup("text-to-speech")
            .WithTags("TextToSpeech")
            .RequireAuthorization();
        
        textToSpeechGroup.MapTextToSpeechWriterEndpoints();
        textToSpeechGroup.MapTextToSpeechReaderEndpoints();
    }
}