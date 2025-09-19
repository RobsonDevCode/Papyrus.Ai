using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface INoteDomainMapper
{
    NoteRequestModel MapToDomain(WriteNoteRequest request);
    NoteModel MapToDomain(Guid id, Guid groupId, LlmResponseModel llmResponseModel, int pageNumber);
    NoteModel MapToDomain(LlmResponseModel llmResponseModel, PageModel page);
    EditNoteRequestModel MapToDomain(EditNoteRequest request);
    WriteImageNoteRequestModel MapToDomain(WriteImageNoteRequest request);
    NoteModel MapToDomain(Note note);
    List<NoteModel> Map(List<Note> notes);
    PagedResponseModel<NoteModel> Map(PagedResponse<Note> response, int page, int size);
}