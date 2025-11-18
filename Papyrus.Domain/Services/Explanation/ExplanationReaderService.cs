using Microsoft.Extensions.Logging;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Clients.Prompts;
using Papyrus.Domain.Exceptions;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Explanation;
using Papyrus.Domain.Services.Interfaces.Images;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;

namespace Papyrus.Domain.Services.Explanation;

public sealed class ExplanationReaderService : IExplanationReaderService
{
    private readonly IPageReader _pageReader;
    private readonly IImageReaderService _imageReaderService;
    private readonly IPapyrusAiClient _client;
    private readonly ILogger<ExplanationReaderService> _logger;

    public ExplanationReaderService(IPageReader pageReader,
        IImageReaderService imageReaderService,
        IPapyrusAiClient client,
        ILogger<ExplanationReaderService> logger)
    {
        _pageReader = pageReader;
        _imageReaderService = imageReaderService;
        _client = client;
        _logger = logger;
    }

    public async Task<string> GetAsync(CreateExplanationRequestModel request, CancellationToken cancellationToken)
    {
        var page = await _pageReader.GetAsync(new PageReaderFilters
        {
            DocumentGroupId = request.DocumentGroupId,
            PageNumber = request.PageNumber,
            UserId = request.UserId
        }, cancellationToken);

        if (page is null)
        {
            throw new PageNotFoundException(
                $"Page {request.PageNumber} on group {request.DocumentGroupId} for user {request.UserId} was not found.");
        }

        if (!string.IsNullOrWhiteSpace(page.ImageUrl))
        {
            _logger.LogInformation("Prompting with image for user {userId}", request.UserId);
            return await GenerateImageExplanationAsync(request, page,cancellationToken);
        }

        if (string.IsNullOrEmpty(page.Content))
        {
            throw new InvalidOperationException("Page has no image reference or page content!");
        }

        var prompt = PromptGenerator.ExplanationPrompt(false, request.TextToExplain, page.Content);
        var response = await _client.PromptAsync(prompt, null, null, cancellationToken);
        return response.ExtractResponse();
    }


    private async Task<string> GenerateImageExplanationAsync(CreateExplanationRequestModel request,
        Page page,
        CancellationToken cancellationToken)
    {
        var prompt = PromptGenerator.ExplanationPrompt(true, request.TextToExplain, page.Content!);
        var imageId = new Uri(page.ImageUrl ?? "").Segments.Last();
        await using var imageStream = await _imageReaderService.GetById(Guid.Parse(imageId), cancellationToken);
        if (imageStream is null)
        {
            throw new InvalidOperationException("Image stream was not found.");
        }

        var imageBytes = imageStream.ToArray();
        var response = await _client.PromptAsync(prompt, null,
            Convert.ToBase64String(imageBytes), cancellationToken);

        return response.ExtractResponse();
    }
}