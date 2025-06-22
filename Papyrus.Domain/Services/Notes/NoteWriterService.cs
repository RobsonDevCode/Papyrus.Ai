using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Clients.Prompts;
using Papyrus.Domain.Exceptions;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Perstistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Domain.Services.Notes;

public sealed class NoteWriterService : INoteWriterService
{
    private readonly IDocumentReaderService _documentReaderService;
    private readonly INoteReader _noteReader;
    private readonly IPapyrusAiClient _papyrusAiClient;
    private readonly INoteWriter _noteWriter;
    private readonly ILogger<NoteWriterService> _logger;
    private readonly IMapper _mapper;

    public NoteWriterService(IDocumentReaderService documentReaderService,
        INoteReader noteReader,
        IPapyrusAiClient papyrusAiClient,
        INoteWriter noteWriter,
        ILogger<NoteWriterService> logger,
        IMapper mapper)
    {
        _documentReaderService = documentReaderService;
        _noteReader = noteReader;
        _papyrusAiClient = papyrusAiClient;
        _noteWriter = noteWriter;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NoteModel> WriteNoteAsync(NoteRequestModel request, CancellationToken cancellationToken)
    {
        var page = await _documentReaderService.GetByGroupIdAsync(request.DocumentTypeId, request.Page,
            cancellationToken);

        if (page is null)
        {
            throw new DocumentNotFoundExeception("No document found");
        }

        if (request.ImageReference is not null)
        {
            _logger.LogInformation("Writing an image focused note");
            return await WriteImageFocusedNoteAsync((int)request.ImageReference, page, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            _logger.LogInformation("Writing note on selected text");
           return await WriteTextSelectedNoteAsync(request.Text, page,cancellationToken);
        }

        _logger.LogInformation("Writing note on selected page");
        var prompt = PromptGenerator.PagePrompt(page.DocumentName);

        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, page.Image, cancellationToken);

        var note = _mapper.MapToPersistance(llmResponse, page);

        await _noteWriter.SaveNoteAsync(note, cancellationToken);

        return _mapper.MapToDomain(note);
    }

    public async Task<NoteModel> UpdateNoteAsync(EditNoteRequestModel request, CancellationToken cancellationToken)
    {
        //manual update by the user no need for any llm interaction and we can directly  
        var editedNote = await _noteWriter.UpdateNoteAsync(request.Id, request.EditedNote, cancellationToken);
        return _mapper.MapToDomain(editedNote);
    }

    public async Task<NoteModel> UpdateNoteWithPromptAsync(UpdateNoteRequestModel request,
        CancellationToken cancellationToken)
    {
        var noteTask = _noteReader.GetNoteAsync(request.NoteId, cancellationToken);
        var pageTask = _documentReaderService.GetByIdAsync(request.DocumentId, cancellationToken);

        var tasks = new Task[] { noteTask, pageTask };

        await Task.WhenAll(tasks);

        var note = await noteTask;
        if (note is null)
        {
            throw new NoteNotFoundException($"{request.NoteId} not found");
        }

        var page = await pageTask;
        if (page is null)
        {
            throw new DocumentNotFoundExeception($"{request.NoteId} not found");
        }

        var prompt = PromptGenerator.ImproveNotePrompt(note.Text, request.Prompt, page.DocumentName, page.Content);
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, page.Image, cancellationToken);

        return _mapper.MapToDomain(note.Id, page.DocumentGroupId, llmResponse, page.PageNumber);
    }

    private async Task<NoteModel> WriteImageFocusedNoteAsync(int imageReference, PageModel page,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(page.Image))
        {
            throw new Exception($"No image found on {page.PageNumber} in {page.DocumentName}");
        }

        if (imageReference > page.ImageCount)
        {
            throw new Exception($"Image reference {imageReference} is out of range");
        }
        
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(PromptGenerator.ImageFocusedNote(page.DocumentName, imageReference), page.Image, cancellationToken);

        var note = _mapper.MapToPersistance(llmResponse, page);
        await _noteWriter.SaveNoteAsync(note, cancellationToken);

        return _mapper.MapToDomain(note);
    }

    private async Task<NoteModel> WriteTextSelectedNoteAsync(string text, PageModel page,
        CancellationToken cancellationToken)
    {
        var prompt = string.IsNullOrWhiteSpace(page.Image)
            ? PromptGenerator.BasicNotePrompt(page.DocumentName, text)
            : PromptGenerator.PromptTextWithImage(page.DocumentName, text);

        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, page.Image, cancellationToken);
        var note = _mapper.MapToPersistance(llmResponse, page);
        await _noteWriter.SaveNoteAsync(note, cancellationToken);

        return _mapper.MapToDomain(note);
    }
}