using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Mappers.Domain;

public interface ILabelsDomainMapper
{
    Labels? MapToDomain(Persistance.Interfaces.Contracts.Labels? label);
}