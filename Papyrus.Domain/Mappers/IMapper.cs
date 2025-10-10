using Papyrus.Domain.Mappers.Domain;
using Papyrus.Domain.Mappers.Persistence;
using Papyrus.Domain.Mappers.Responses;
using Papyrus.Domain.Mappers.Responses.Domain;

namespace Papyrus.Domain.Mappers;

public interface IMapper : IPageResponseMapper, IPageDomainMapper,
    INoteDomainMapper, INoteResponseMapper, INotePersistenceMapper,
    IImagePersistenceMapper, IPromptDomainMapper, IImageDomainMapper,
    IBookmarkDomainMapper, IBookmarkPersistenceMapper, IBookmarkResponseMapper, 
    IAudioSettingsDomainMapper, IAudioSettingPersistenceMapper, IAudioSettingsResponseMapper,
    IDocumentResponseMapper, IDocumentPersistenceMapper, IDocumentDomainMapper,
    IPaginationResponseMapper,
    IVoiceResponseMapper, IVoiceDomainMapper, IVoicePersistenceMapper,
    ILabelResponseMapper, ILabelsDomainMapper,
    IAudioDomainMapper,
    IAudioAlignmentDomainMapper, IAudioAlignmentPersistenceMapper, IAudioAlignmentResponseMapper,
    IUserDomainMapper ,IUserPersistenceMapper;