﻿using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Services.Interfaces.Notes;

public interface INoteWriterService
{
    Task<NoteModel> WriteNoteAsync(NoteRequestModel request, CancellationToken cancellationToken);
    
    Task<NoteModel> UpdateNoteAsync(EditNoteRequestModel request, CancellationToken cancellationToken);
    Task<NoteModel> UpdateNoteWithPromptAsync(UpdateNoteRequestModel request, CancellationToken cancellationToken);
}