using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Responses.Domain;

public interface IPageDomainMapper
{
    PageModel MapToDomain(Page page);
}