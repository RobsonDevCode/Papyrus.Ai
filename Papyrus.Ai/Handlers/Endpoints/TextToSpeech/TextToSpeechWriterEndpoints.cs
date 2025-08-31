namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechWriterEndpoints
{
   internal static void MapTextToSpeachWriterEndpoints(this WebApplication app)
   {
      var textToSpeachGroup = app.MapGroup("text-to-speech");
   }
   
}