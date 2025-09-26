using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Domain.Models.Voices;

namespace Papyrus.Domain.Services.Interfaces;

public interface IVoiceReaderService
{
    Task<PagedResponseModel<VoiceModel>> GetVoices(VoiceSearchModel filter, CancellationToken cancellationToken);
}