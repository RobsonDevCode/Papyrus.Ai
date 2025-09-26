using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;
using Labels = Papyrus.Persistance.Interfaces.Contracts.Labels;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public Voice MapToPersistence(VoiceResponseModel voiceSearchModel)
    {
        return new Voice
        {
            VoiceId = voiceSearchModel.VoiceId,
            Name = voiceSearchModel.Name,
            Category = voiceSearchModel.Category,
            Description = voiceSearchModel.Description,
            PreviewUrl = voiceSearchModel.PreviewUrl,
            Labels = MapToPersistence(voiceSearchModel.Labels),
            Settings = new VoiceSettings
            {
                Speed = voiceSearchModel.Settings?.Speed ?? 1.0,
                Stability = voiceSearchModel.Settings?.Stability ?? 0.5,
                UseSpeakerBoost = voiceSearchModel.Settings?.UseSpeakerBoost ?? false
            },
            IsMixed = voiceSearchModel.IsMixed,
            CreatedAtUnix = voiceSearchModel.CreatedAtUnix
        };
    }

    public IEnumerable<Voice> MapToPersistence(IEnumerable<VoiceResponseModel> voiceSearchModels)
    {
        return voiceSearchModels.Select(MapToPersistence);
    }

    public VoiceSearch MapToPersistence(VoiceSearchModel filter)
    {
        return new VoiceSearch
        {
            Pagination = new PaginationOptions(filter.Pagination.Page, filter.Pagination.Size),
            Accent = filter.Accent,
            UseCase = filter.UseCase,
            SearchTerm = filter.SearchTerm,
            Gender = filter.Gender
        };
    }

    private Labels? MapToPersistence(Papyrus.Domain.Models.Client.Audio.Labels? labels)
    {
        if (labels == null)
        {
            return null;
        }

        return new Labels
        {
            Accent = labels.Accent,
            Descriptive = labels.Descriptive,
            Age = labels.Age,
            Gender = labels.Gender,
            Language = labels.Language,
            UseCase = labels.UseCase,
        };
    }
    
}