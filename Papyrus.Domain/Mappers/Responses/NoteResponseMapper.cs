using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Pagination;

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

    public PagedResponse<NoteResponse> MapToResponse(PagedResponseModel<NoteModel> pagedResponse)
    {
        return new PagedResponse<NoteResponse>
        {
            Items = MapToResponseArray(pagedResponse.Items),
            Pagination = new Pagination
            {
                Size = pagedResponse.Pagination.Size,
                Page = pagedResponse.Pagination.Page,
                Total = pagedResponse.Pagination.TotalPages,
            }
        };
    }

    public NoteResponse[] MapToResponseArray(NoteModel[] notes)
    {
        return notes.Select(MapToResponse).ToArray();
    }
    
}