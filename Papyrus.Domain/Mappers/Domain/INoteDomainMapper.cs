using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface INoteDomainMapper
{
    NoteRequestModel MapToDomain(WriteNoteRequest request);
    NoteModel MapToDomain(Guid id, Guid groupId, LlmResponse llmResponse, int pageNumber);
    EditNoteRequestModel MapToDomain(EditNoteRequest request);
    UpdateNoteRequestModel MapToDomain(AddToNoteRequest request);
    WriteImageNoteRequestModel MapToDomain(WriteImageNoteRequest request);
    NoteModel MapToDomain(Note note);
    List<NoteModel> Map(List<Note> notes);
    PagedResponseModel<NoteModel>? Map(PagedResponse<Note> response, int page, int size);
}