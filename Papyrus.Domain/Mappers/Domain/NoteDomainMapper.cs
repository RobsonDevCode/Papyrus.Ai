using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;
public partial class Mapper
{
    public NoteRequestModel MapToDomain(WriteNoteRequest request)
    {
        return new NoteRequestModel
        {
            DocumentTypeId = request.DocumentGroupId,
            Page = request.Page,
            Text = request.Text
        };
    }

    public WriteImageNoteRequestModel MapToDomain(WriteImageNoteRequest request)
    {
        return new WriteImageNoteRequestModel
        {
            DocumentGroupId = request.DocumentGroupId,
            Page = request.Page,
            ImageReference = request.ImageReference
        };
    }

    public NoteModel MapToDomain(Note note)
    {
        return new NoteModel
        {
            Id = note.Id,
            DocumentGroupId = note.DocumentGroupId,
            Note = note.Text,
            PageReference = note.RelatedPage,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public List<NoteModel> Map(List<Note> notes)
    {
        return notes.Select(MapToDomain).ToList();
    } 

    public PagedResponseModel<NoteModel>? Map(PagedResponse<Note> response, int page, int size)
    {
        return new PagedResponseModel<NoteModel>
        {
            Items = response.Data.Select(MapToDomain).ToArray(),
            Pagination = new PaginationModel
            {
                Page = page,
                Size = size,
                TotalPages = response.TotalPages,
            }
        };
    }
}