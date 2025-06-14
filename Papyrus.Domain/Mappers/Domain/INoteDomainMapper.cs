using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface INoteDomainMapper
{
    NoteRequestModel MapToDomain(WriteNoteRequest request);

    NoteModel MapToDomain(Note note);
}