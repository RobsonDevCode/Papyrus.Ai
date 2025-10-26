namespace Papyrus.Ai.Handlers.Endpoints.Voices;

internal static class VoiceEndpointsMapper
{
    internal static void MapVoiceEndpoints(this WebApplication app)
    {
        var voiceEndpointsGroup = app.MapGroup("voice")
            .WithTags("Voice-endpoints")
            .RequireAuthorization();
        
        voiceEndpointsGroup.MapVoiceReaderEndpoints();
    }
}