using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface INotePersistenceMapper
{
   Note MapToPersistence(LlmResponse response, PageModel pageModel);
}