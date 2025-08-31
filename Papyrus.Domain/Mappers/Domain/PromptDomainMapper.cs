using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public List<PromptModel> Map(List<Prompt> prompts)
    {
        return prompts.Select(Map).ToList();
    }

    public PromptModel Map(Prompt prompt)
    {
        return new PromptModel
        {
            Id = prompt.Id,
            ChatId = prompt.ChatId,
            NoteId = prompt.NoteId,
            Prompt = prompt.UserPrompt,
            Response = prompt.Response,
            CreatedAt = prompt.CreatedAt
        };
    }
}