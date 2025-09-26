using Papyrus.Api.Contracts.Contracts;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public LabelsResponse? MapToResponse(Labels? labels)
    {
        if (labels == null) return null;
        
        return new LabelsResponse
        {
            Accent = labels.Accent,
            Descriptive = labels.Descriptive,
            Age = labels.Age,
            Gender = labels.Gender,
            Language = labels.Language,
            UseCase = labels.UseCase
        };
    }
}