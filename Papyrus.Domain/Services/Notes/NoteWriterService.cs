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

    public Task<NoteModel> WriteNotesAsync(NoteRequestModel request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}