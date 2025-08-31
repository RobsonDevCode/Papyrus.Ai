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
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistance.Interfaces.Writer;

namespace Papyrus.Domain.Services.Notes;

public sealed class NoteWriterService : INoteWriterService
{
    private readonly IDocumentReaderService _documentReaderService;
    private readonly IPromptHistoryReader _promptHistoryReader;
    private readonly IPromptHistoryWriter _promptHistoryWriter;
    private readonly IPapyrusAiClient _papyrusAiClient;
    private readonly INoteWriter _noteWriter;
    private readonly ILogger<NoteWriterService> _logger;
    private readonly IMapper _mapper;

    public NoteWriterService(IDocumentReaderService documentReaderService,
        IPromptHistoryReader promptHistoryReader,
        IPromptHistoryWriter promptHistoryWriter,
        IPapyrusAiClient papyrusAiClient,
        INoteWriter noteWriter,
        ILogger<NoteWriterService> logger,
        IMapper mapper)
    {
        _documentReaderService = documentReaderService;
        _promptHistoryReader = promptHistoryReader;
        _promptHistoryWriter = promptHistoryWriter;
        _papyrusAiClient = papyrusAiClient;
        _noteWriter = noteWriter;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NoteModel> WriteNoteAsync(NoteRequestModel request, CancellationToken cancellationToken)
    {
        var page = await _documentReaderService.GetByIdAsync(request.PageId, cancellationToken);

        if (page is null)
        {
            throw new DocumentNotFoundExeception("No document found");
        }

        if (request.Prompt is not null)
        {
            _logger.LogInformation("Writing prompt driven note");
            return await WriteNoteFromPromptAsync(request.Prompt, page, cancellationToken);
        }

        if (request.ImageReference is not null)
        {
            _logger.LogInformation("Writing an image focused note");
            return await WriteImageFocusedNoteAsync((int)request.ImageReference, page, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.Text))
        {
            _logger.LogInformation("Writing note on selected text");
            return await WriteTextSelectedNoteAsync(request.Text, page, cancellationToken);
        }

        _logger.LogInformation("Writing note on selected page");
        var prompt = PromptGenerator.PagePrompt(page.DocumentName);
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(prompt, images: page.Image, cancellationToken: cancellationToken);
        
        var note = _mapper.MapToPersistence(llmResponse, page);
        var chatId = Guid.NewGuid();
        note.ChatId = chatId;
        
        var promptToSave = new Prompt
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            NoteId = note.Id,
            UserPrompt = prompt,
            Response = note.Text,
            CreatedAt = note.CreatedAt
        };

        var saveNote = _noteWriter.SaveNoteAsync(note, cancellationToken);
        var savePrompt = _promptHistoryWriter.InsertAsync(promptToSave, cancellationToken);

        await Task.WhenAll(saveNote, savePrompt);

        return _mapper.MapToDomain(note);
    }

    public async Task<NoteModel> UpdateNoteAsync(EditNoteRequestModel request, CancellationToken cancellationToken)
    {
        var editedNote = await _noteWriter.UpdateNoteAsync(request.Id, request.EditedNote, cancellationToken);
        if (editedNote is null)
        {
            throw new NoteNotFoundException($"Note {request.Id} not found");
        }
        
        return _mapper.MapToDomain(editedNote);
    }


    private async Task<NoteModel> WriteNoteFromPromptAsync(PromptRequestModel request, PageModel page,
        CancellationToken cancellationToken)
    {
        var promptHistory = await _promptHistoryReader.GetHistory(request.NoteId, cancellationToken);
        var mappedPrompts = _mapper.Map(promptHistory);
        
        var llmResponse = await _papyrusAiClient.CreateNoteAsync(request.Text, mappedPrompts, images: page.Image,
            cancellationToken: cancellationToken);

        var promptToSave = new Prompt
        {
            Id = Guid.NewGuid(),
            ChatId = promptHistory.Select(x => x.ChatId).First(),
            NoteId = request.NoteId,
            CreatedAt = DateTime.UtcNow,
            UserPrompt = request.Text,
            Response = llmResponse.ExtractResponse()
        };

        await _promptHistoryWriter.InsertAsync(promptToSave, cancellationToken);
        return _mapper.MapToDomain(llmResponse, page);
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

        var prompt = PromptGenerator.ImageFocusedNote(page.DocumentName, imageReference);
        var llmResponse =
            await _papyrusAiClient.CreateNoteAsync(prompt, images: page.Image, cancellationToken: cancellationToken);
        
        var note = _mapper.MapToPersistence(llmResponse, page);

        var promptToSave = new Prompt
        {
            Id = Guid.NewGuid(),
            ChatId = Guid.NewGuid(),
            NoteId = note.Id,
            CreatedAt = DateTime.UtcNow,
            UserPrompt = prompt,
            Response = note.Text
        };

        var saveNoteTask = _noteWriter.SaveNoteAsync(note, cancellationToken);
        var savePromptTask = _promptHistoryWriter.InsertAsync(promptToSave, cancellationToken);

        await Task.WhenAll(saveNoteTask, savePromptTask);

        return _mapper.MapToDomain(note);
    }

    private async Task<NoteModel> WriteTextSelectedNoteAsync(string text, PageModel page,
        CancellationToken cancellationToken)
    {
        var prompt = string.IsNullOrWhiteSpace(page.Image)
            ? PromptGenerator.BasicNotePrompt(page.DocumentName, text)
            : PromptGenerator.PromptTextWithImage(page.DocumentName, text);

        var llmResponse =
            await _papyrusAiClient.CreateNoteAsync(prompt, images: page.Image, cancellationToken: cancellationToken);
        var note = _mapper.MapToPersistence(llmResponse, page);

        var promptToSave = new Prompt
        {
            Id = Guid.NewGuid(),
            ChatId = Guid.NewGuid(),
            NoteId = note.Id,
            CreatedAt = DateTime.UtcNow,
            UserPrompt = prompt,
            Response = note.Text
        };

        var saveNoteTask = _noteWriter.SaveNoteAsync(note, cancellationToken);
        var savePromptTask = _promptHistoryWriter.InsertAsync(promptToSave, cancellationToken);

        await Task.WhenAll(saveNoteTask, savePromptTask);
        return _mapper.MapToDomain(note);
    }
}