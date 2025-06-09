using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Mappers;
public partial class Mapper
{
    public NoteRequestModel MapToDomain(Guid documentId, WriteNotesOptions options)
    {
        return new NoteRequestModel
        {
            DocumentTypeId = documentId,
            Page = options.Page,
            Text = options.Text
        };
    }
}