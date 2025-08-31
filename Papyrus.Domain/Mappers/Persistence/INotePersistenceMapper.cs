using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface INotePersistenceMapper
{
   Note MapToPersistence(LlmResponseModel responseModel, PageModel pageModel);
}