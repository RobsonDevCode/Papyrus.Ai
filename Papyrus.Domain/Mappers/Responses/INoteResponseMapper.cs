﻿using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers;

public interface INoteResponseMapper
{
    NoteResponse MapToResponse(NoteModel note);
    
    List<NoteResponse> MapToResponse(NoteModel[] pagedResponse);
    
    List<NoteResponse> MapToResponse(List<NoteModel> pagedResponse);

}