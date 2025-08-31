using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IPromptDomainMapper
{
    List<PromptModel> Map(List<Prompt> prompts);
    
    PromptModel Map(Prompt prompt);
}