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
    }

    internal static async Task<Results<Ok<NoteResponse>, BadRequest<string>>> WriteNote(
        [FromRoute] Guid documentGroupId,
        [AsParameters] WriteNotesOptions options,
        [FromServices] IValidator<WriteNotesOptions> noteValidator,
        [FromServices] INoteWriterService writerService,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Write Note",
            [Filter] = (string.IsNullOrWhiteSpace(options.Text) ? options.Page.ToString() : options.Text) ??
                       string.Empty
        });

        var validationResult = await noteValidator.ValidateAsync(options, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        var mappedRequest = mapper.MapToDomain(documentGroupId, options);
        var result = await writerService.WriteNotesAsync(mappedRequest, cancellationToken);

        var mappedToResponse = mapper.MapToResponse(result);
        return TypedResults.Ok(mappedToResponse);
    }
}