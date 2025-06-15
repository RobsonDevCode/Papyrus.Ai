using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Domain.Services.Notes;

public sealed class NoteWriterService : INoteWriterService
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

    public async Task<NoteModel> WriteNotesAsync(NoteRequestModel request, CancellationToken cancellationToken)
    {
        var page = await _documentReaderService.GetPageByIdAsync(request.DocumentTypeId, request.Page, cancellationToken);

        if (page is null)
        {
            throw new Exception("No document found");
        }
        
        var prompt = string.IsNullOrWhiteSpace(request.Text) ? page.Content : request.Text;

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
}