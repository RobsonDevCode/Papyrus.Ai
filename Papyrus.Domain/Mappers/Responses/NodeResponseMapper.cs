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
            Text = note.Text,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }
}