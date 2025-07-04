﻿using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;

namespace Papyrus.Domain.Services.Interfaces.Notes;

public interface INoteReaderService
{
    ValueTask<NoteModel?> GetNoteAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponseModel<NoteModel>?> GetNotesAsync(Guid id, PaginationRequestModel options, CancellationToken cancellationToken);

    ValueTask<List<NoteModel>?> GetNotesOnPageAsync(Guid documentGroupId, int pdfPage,
        CancellationToken cancellationToken);
}