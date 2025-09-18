using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.TextToSpeech;

internal static class TextToSpeechWriterEndpoints
{
   internal static void MapTextToSpeechWriterEndpoints(this RouteGroupBuilder app)
   {
      app.MapPost("", Create);
      app.MapPost("setting", AddSettings);
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
         [Operation] = "UpsertAsync AudioBook",
         [DocumentGroupId] = request.DocumentGroupId,
         [Filter] = request
      });
      
      var validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
      {
         var errors = string.Join(" | " + validationResult.Errors.Select(x => x.ErrorMessage));
         return TypedResults.BadRequest(errors);
      }
      
      var audioBytes = await audioBookWriterService.CreateAsync(request.DocumentGroupId, request.VoiceId, request.Pages, cancellationToken);
      if (audioBytes.Length == 0)
      {
         throw new InvalidOperationException("Audio book cannot have an empty audio file.");
      }
      
      return TypedResults.File(audioBytes, "audio/mpeg","audio-to-speech.mp3");
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