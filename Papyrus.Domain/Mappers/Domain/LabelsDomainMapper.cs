using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public Labels? MapToDomain(Persistance.Interfaces.Contracts.Labels? label)
    {
        if (label == null)
        {
            return null;
        }

        return new Labels
        {
            Accent = label.Accent,
            Descriptive = label.Descriptive,
            Age = label.Age,
            Gender = label.Gender,
            Language = label.Language,
            UseCase = label.UseCase
        };
    }
}