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
        string name;
        string prompt;
        
        if(string.IsNullOrWhiteSpace(request.Text))
        {
            var response =
                await _documentReaderService.GetPageByIdAsync(request.DocumentTypeId, request.Page, cancellationToken);

            if (response is null)
            {
                throw new Exception("No document found");
            }

            name = response.DocumentName;
            prompt = response.Content;
        }
        else
        {
            name = await _documentReaderService.GetDocumentNameAsync(request.DocumentTypeId,cancellationToken) ?? "";
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("No document found");
            }
            
            prompt = request.Text;
        }
        
        var llmResonse = await _papyrusAiClient.CreateNoteAsync(name, prompt, cancellationToken);
        
        //TODO save note
        throw new NotImplementedException();
    } 
}