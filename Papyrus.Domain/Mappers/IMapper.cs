using Papyrus.Domain.Mappers.Domain;
using Papyrus.Domain.Mappers.Responses.Domain;

namespace Papyrus.Domain.Mappers;

public interface IMapper : IPageResponseMapper, IPageDomainMapper, INoteDomainMapper, INoteResponseMapper,
    INotePersistenceMapper, IImagePersistenceMapper, IPromptDomainMapper, IImageDomainMapper;