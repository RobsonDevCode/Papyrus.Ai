using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Mappers.Domain;

public interface INoteDomainMapper
{
    NoteRequestModel MapToDomain(Guid documentId, WriteNotesOptions options);
}