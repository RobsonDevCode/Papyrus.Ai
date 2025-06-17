using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Clients.Prompts;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Domain.Services.Notes;

public sealed partial class NoteWriterService : INoteWriterService
{
    private readonly IDocumentReaderService _documentReaderService;
    private readonly IPapyrusAiClient _papyrusAiClient;
    private readonly INoteWriter _noteWriter;
    private readonly ILogger<NoteWriterService> _logger;
    private readonly IMapper _mapper;

    public NoteWriterService(IDocumentReaderService documentReaderService, IPapyrusAiClient papyrusAiClient,
        INoteWriter noteWriter, ILogger<NoteWriterService> logger, IMapper mapper)
    {
        _documentReaderService = documentReaderService;
        _papyrusAiClient = papyrusAiClient;
        _noteWriter = noteWriter;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NoteModel> WriteNoteAsync(NoteRequestModel request, CancellationToken cancellationToken)
    {
        var page = await _documentReaderService.GetPageByIdAsync(request.DocumentTypeId, request.Page, cancellationToken);

        if (page is null)
        {
            throw new Exception("No document found");
        }

        if (request.ImageReference is not null)
        {
            return await WriteImageFocusedNoteAsync((int)request.ImageReference, page, cancellationToken);
        }
        
        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            return await WriteTextSelectedNoteAsync(request.Text, page, cancellationToken);
        }
        
        var prompt = page.Images is { Count: > 0 } 
            ? PromptGenerator.PromptTextWithImage(page.DocumentName, page.Content)
            : PromptGenerator.BasicNotePrompt(page.DocumentName, page.Content);
        
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(page.DocumentName, prompt, page.Images, cancellationToken);

        var mapped = _mapper.MapToPersistance(llmResponse, page); 

        await _noteWriter.SaveNoteAsync(mapped, cancellationToken);

        return new NoteModel
        {
            Id = mapped.Id,
            DocumentGroupId = page.DocumentGroupId,
            Note = llmResponse.Repsonse,
            CreatedAt = llmResponse.CreatedAt,
            UpdatedAt = llmResponse.CreatedAt,
            PageReference = page.PageNumber
        };
    }

    private async Task<NoteModel> WriteImageFocusedNoteAsync(int imageReference, PageModel page, CancellationToken cancellationToken)
    {
        if (page.Images is not { Count: > 0 })
        {
            throw new Exception($"No images found on {page.DocumentName}");
        }
        
        throw new NotImplementedException();
    }
    
    private async Task<NoteModel> WriteTextSelectedNoteAsync(string text, PageModel page,
        CancellationToken cancellationToken)
    {
        var images = new List<ImageModel>();
        if (TryExtractImageReferences(text, out var imagesReferences) && page.Images != null)
        {
            images.AddRange(imagesReferences.Select(imageReference => page.Images[imageReference]));
        }
        
        var prompt = images.Count > 0 
            ? PromptGenerator.PromptTextWithImage(page.DocumentName,text)
            : PromptGenerator.BasicNotePrompt(page.DocumentName, text);
        
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(page.DocumentName, prompt, images, cancellationToken);
        var mapped = _mapper.MapToPersistance(llmResponse, page);
        await _noteWriter.SaveNoteAsync(mapped, cancellationToken);

        return new NoteModel
        {
            Id = mapped.Id,
            DocumentGroupId = page.DocumentGroupId,
            Note = llmResponse.Repsonse,
            CreatedAt = llmResponse.CreatedAt,
            UpdatedAt = llmResponse.CreatedAt,
            PageReference = page.PageNumber
        };
    }
    private bool TryExtractImageReferences(string text, out List<int> imagesRefs)
    {
        try
        {
            var matches = ImageRegex().Matches(text);
            var numbers = new List<int>();
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int imageRef))
                {
                    numbers.Add(imageRef);
                }
            }

            imagesRefs = numbers;
            return numbers.Count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract image references");
            imagesRefs = [];           
            return false;
        }
    }

    [GeneratedRegex(@"\$\$IMAGE_(\d+)\$\$")]
    private static partial Regex ImageRegex();
}