using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
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
}