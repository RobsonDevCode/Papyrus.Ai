using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Responses.Domain;

public interface IPageDomainMapper
{
    PageModel MapToDomain(Page page);
    IEnumerable<PageModel> MapToDomain(IEnumerable<Page?> pages);
}