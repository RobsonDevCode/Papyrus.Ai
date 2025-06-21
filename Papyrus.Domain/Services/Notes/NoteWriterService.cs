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
using Papyrus.Perstistance.Interfaces.Contracts;
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
            //return await WriteImageFocusedNoteAsync((int)request.ImageReference, page, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.Text))
        {
           // return await WriteTextSelectedNoteAsync(request.Text, page, cancellationToken);
        }

        //write note on entire page
        var prompt = page.Images is { Length: > 0 }
            ? PromptGenerator.PromptTextWithImage(page.DocumentName, page.Content)
            : PromptGenerator.BasicNotePrompt(page.DocumentName, page.Content);

        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, page.Images, cancellationToken);

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
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, page.Images, cancellationToken);

        return _mapper.MapToDomain(note.Id, page.DocumentGroupId, llmResponse, page.PageNumber);
    }

    /*private async Task<NoteModel> WriteImageFocusedNoteAsync(int imageReference, PageModel page,
        CancellationToken cancellationToken)
    {
        if (page.Images is not { Length: > 0 })
        {
            throw new Exception($"No images found on {page.PageNumber} in {page.DocumentName}");
        }

        var image = page.Images[imageReference];

        var llmResponse = await _papyrusAiClient.CreateNoteAsync(
            PromptGenerator.ImageFocusedNote(page.DocumentName, page.Content), image, cancellationToken);

        var note = _mapper.MapToPersistance(llmResponse, page);
        await _noteWriter.SaveNoteAsync(note, cancellationToken);

        return _mapper.MapToDomain(note);
    }

    private async Task<NoteModel> WriteTextSelectedNoteAsync(string text, PageModel page,
        CancellationToken cancellationToken)
    {
        var images = new List<ImageModel>();
        if (PdfExtensions.TryExtractImageReferences(text, out var imagesReferences)
            && page.Images != null)
        {
           // images.AddRange(imagesReferences.Select(imageReference => page.Images[imageReference]));
        }
        else
        {
            _logger.LogInformation("No images found on {pageNumber} in {documentName}", page.PageNumber,
                page.DocumentName);
        }

        var prompt = images.Count > 0
            ? PromptGenerator.PromptTextWithImage(page.DocumentName, text)
            : PromptGenerator.BasicNotePrompt(page.DocumentName, text);

        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, images.ToArray(), cancellationToken);
        var note = _mapper.MapToPersistance(llmResponse, page);
        await _noteWriter.SaveNoteAsync(note, cancellationToken);

        return _mapper.MapToDomain(note);
    }*/
}