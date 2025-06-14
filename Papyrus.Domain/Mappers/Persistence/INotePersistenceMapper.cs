using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface INotePersistenceMapper
{
   Note MapToPersistance(OllamaResponse response, PageModel pageModel);
}