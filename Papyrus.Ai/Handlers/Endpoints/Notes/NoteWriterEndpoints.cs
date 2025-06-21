using System.IO.Compression;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.Notes;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Notes;

internal static class NoteWriterEndpoints
{
    internal static void MapNoteWriterEndpoints(this WebApplication app)
    {
        var noteGroup = app.MapGroup("notes");
        
        noteGroup.MapPost("", WriteNote);
        noteGroup.MapPost("redo", AddToNoteWithPrompt);
        noteGroup.MapPost("update", UpdateNote);
    }

    private static async Task<Results<Ok<NoteResponse>, BadRequest<string>>> WriteNote(
        [FromBody] WriteNoteRequest request,
        [FromServices] IValidator<WriteNoteRequest> noteValidator,
        [FromServices] INoteWriterService writerService,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Write Note",
            [Filter] = request
        });

        logger.LogInformation("Starting writing note request");
        
        var validationResult = await noteValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var mappedRequest = mapper.MapToDomain(request);
        var result = await writerService.WriteNoteAsync(mappedRequest, cancellationToken);

        var mappedToResponse = mapper.MapToResponse(result);
        
        logger.LogInformation("Successfully wrote and saved note");
        
        return TypedResults.Ok(mappedToResponse);
    }

    private static async Task<Results<Ok<NoteResponse>, BadRequest<string>>> AddToNoteWithPrompt(
        [FromBody] AddToNoteRequest request,
        [FromServices] IValidator<AddToNoteRequest> validator,
        [FromServices] INoteWriterService writerService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Update Note with prompt",
            [Filter] = request
        });
        
        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,      
            cts.Token      
        );
        
        logger.LogInformation("Starting update note with prompt request");
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var mappedRequest = mapper.MapToDomain(request);
        var updatedNote = await writerService.UpdateNoteWithPromptAsync(mappedRequest, cancellationToken);
        
        return TypedResults.Ok(mapper.MapToResponse(updatedNote));
    }

    private static async Task<Results<Ok<NoteResponse>, BadRequest<string>>> UpdateNote(
    [FromBody] EditNoteRequest request,
    [FromServices] IValidator<EditNoteRequest> noteValidator, 
    [FromServices] INoteWriterService writerService, 
    [FromServices] IMapper mapper,
    [FromServices] ILoggerFactory loggerFactory,
    CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Edit Note",
            [DocumentGroupId] = request.DocumentGroupId,
            [PdfPage] = request.Page
        });
        
        logger.LogInformation("Starting edit note request");
        var validationResult = await noteValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var mappedRequest = mapper.MapToDomain(request);
        
        var response = await writerService.UpdateNoteAsync(mappedRequest, cancellationToken);
        var mappedToResponse = mapper.MapToResponse(response);
        
        logger.LogInformation("Successfully edited note");
        return TypedResults.Ok(mappedToResponse);
    }
}