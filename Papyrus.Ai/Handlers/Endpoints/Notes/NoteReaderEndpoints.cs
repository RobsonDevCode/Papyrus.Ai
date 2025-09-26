
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces.Notes;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Notes;

internal static class NoteReaderEndpoints
{
    internal static void MapNoteReaderEndpoints(this WebApplication app)
    {
        var noteGroup = app.MapGroup("notes");
     
        noteGroup.MapGet("{noteId}", GetNote);
        noteGroup.MapGet("document/{documentGroupId}", GetNotes);
        noteGroup.MapGet("{documentGroupId}/{pdfPage}", GetNotesForPage);
    }

    private static async Task<Results<Ok<NoteResponse>, NotFound>> GetNote([FromRoute] Guid noteId,
        [FromServices] INoteReaderService noteReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Note",
            [NoteId] = noteId
        });
        
        logger.LogInformation("Getting note");
        
        var response = await noteReaderService.GetNoteAsync(noteId, cancellationToken);
        if (response is null)
        {
            logger.LogWarning("No note was found!");
            return TypedResults.NotFound();
        }        
        
        logger.LogInformation("Successfully retrieved note");
        
        return TypedResults.Ok(mapper.MapToResponse(response));
    }

    private static async Task<Ok<PagedResponse<NoteResponse>>> GetNotes([FromRoute] Guid documentGroupId,
        [AsParameters] PaginationOptions pagination,
        [FromServices] INoteReaderService noteReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Notes",
            [DocumentGroupId] = documentGroupId,
            [Filter] = pagination
        });

        logger.LogInformation("Getting notes page {pageNumber}, page size {pageSize}", pagination.Page,
            pagination.Size);

        var response = await noteReaderService.GetNotesAsync(documentGroupId ,new PaginationRequestModel
        {
            Page = pagination.Page,
            Size = pagination.Size
        }, cancellationToken);
        
      
        var notes = mapper.MapToResponse(response.Items);
        
        return TypedResults.Ok(mapper.MapToResponse(notes, pagination.Page, pagination.Size,
            response.Pagination.TotalPages));
    }

    private static async Task<Ok<List<NoteResponse>>> GetNotesForPage([FromRoute] Guid documentGroupId,
        [FromRoute] int pdfPage,
        [FromServices] INoteReaderService noteReaderService,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.NoteReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = $"Get Notes for Page {pdfPage}",
            [DocumentGroupId] = documentGroupId,
            [PdfPage] = pdfPage,
        });
        
        var response = await noteReaderService.GetNotesOnPageAsync(documentGroupId, pdfPage, cancellationToken);
        if (response is null)
        {
            return TypedResults.Ok(new List<NoteResponse>());
        }
        
        var result = mapper.MapToResponse(response);
        
        return TypedResults.Ok(result);
    }

}