using Papyrus.Api.Contracts.Contracts.Api;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Pagination;

namespace Papyrus.Domain.Mappers;

public interface INoteResponseMapper
{
    NoteResponse MapToResponse(NoteModel note);

    PagedResponse<NoteResponse> MapToResponse(PagedResponseModel<NoteModel> pagedResponse);
    
    PagedResponseModel<NoteResponse> MapToResponse(Perstistance.Interfaces.Contracts.PagedResponse<NoteModel> pagedResponse);
}