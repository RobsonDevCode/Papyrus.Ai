using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Voices;
using Papyrus.Domain.Services.Interfaces;
using static Papyrus.Ai.Constants.LoggingCategories;


namespace Papyrus.Ai.Handlers.Endpoints.Voices;

internal static class VoiceReaderEndpointsMapper
{
    internal static void MapVoiceReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("", GetVoices);
    }

    private static async Task<Ok<PagedResponse<VoiceResponse>>> GetVoices(
        [AsParameters] PaginationOptions pagination,
        [AsParameters] GetVoiceRequest getVoiceRequest,
        [FromServices] IVoiceReaderService voiceReaderService,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.VoiceReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Voices",
            [Filter] = JsonSerializer.Serialize(getVoiceRequest),
        });
        

        logger.LogInformation("Getting Voices for page {pageNumber} size {size}...", pagination.Page, pagination.Size);
        var filter = new VoiceSearchModel
        {
            Pagination = new PaginationRequestModel
            {
                Page = pagination.Page,
                Size = pagination.Size
            },
            SearchTerm = getVoiceRequest.SearchTerm,
            Accent = getVoiceRequest.Accent,
            UseCase = getVoiceRequest.UseCase,
            Gender = getVoiceRequest.Gender
        };
        
        var response = await voiceReaderService.GetVoices(filter, cancellationToken);
        var voices = mapper.MapToResponse(response.Items);
        
        var result = new PagedResponse<VoiceResponse>
        {
            Items = voices.ToArray(),
            Pagination = new Pagination(response.Pagination.Page, response.Pagination.Size, response.Pagination.TotalPages)
        };
        
        return TypedResults.Ok(result);
    }
}