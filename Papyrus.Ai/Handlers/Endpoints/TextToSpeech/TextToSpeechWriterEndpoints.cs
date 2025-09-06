using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechWriterEndpoints
{
   internal static void MapTextToSpeachWriterEndpoints(this RouteGroupBuilder app)
   {
      app.MapPost("", Create);
   }

   private static async Task<Results<FileContentHttpResult, BadRequest<string>>> Create(
      [FromBody] CreateAudioBookRequest request,
      [FromServices] IValidator<CreateAudioBookRequest> requestValidator,
      [FromServices] IAudiobookWriterService audioBookWriterService,
      [FromServices] ILoggerFactory loggerFactory,
      CancellationToken cancellationToken)
   {
      var logger = loggerFactory.CreateLogger(Loggers.TextToSpeech);
      using var _ = logger.BeginScope(new Dictionary<string, object>
      {
         [Operation] = "Create AudioBook",
         [DocumentGroupId] = request.DocumentGroupId,
         [Filter] = request
      });
      
      var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
      {
         var errors = string.Join(" | " + validationResult.Errors.Select(x => x.ErrorMessage));
         return TypedResults.BadRequest(errors);
      }
      
      var audioBytes = await audioBookWriterService.CreateAsync(request.DocumentGroupId, request.VoiceId, cancellationToken);
      if (audioBytes.Length == 0)
      {
         throw new InvalidOperationException("Audio book cannot have an empty audio file.");
      }
      
      return TypedResults.File(audioBytes, "audio/mpeg","audio-to-speech.mp3");
   }
}