using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers;

public partial class Mapper
{
    public NoteResponse MapToResponse(NoteModel note)
    {
        return new NoteResponse
        {
            NoteId = note.Id,
            DocumentGroupId = note.DocumentGroupId,
            Text = note.Note,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public List<NoteResponse> MapToResponse(NoteModel[]? notes)
    {
        return notes == null ? [] : notes.Select(MapToResponse).ToList();
    }
    
    public List<NoteResponse> MapToResponse(List<NoteModel>? notes)
    {
        return notes == null ? [] : notes.Select(MapToResponse).ToList();
    }
    
}