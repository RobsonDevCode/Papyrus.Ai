using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;
public partial class Mapper
{
    public NoteRequestModel MapToDomain(WriteNoteRequest request)
    {
        var result = new NoteRequestModel
        {
            PageId = request.PageId,
            Text = request.Text,
            ImageReference = request.ImageReference,
        };

        if (request.Prompt is not null)
        {
            result.Prompt = new PromptRequestModel
            {
                Text = request.Prompt.Text,
                NoteId = request.Prompt.NoteId
            };
        }
        
        return result;
    }

    public EditNoteRequestModel MapToDomain(EditNoteRequest request)
    {
        return new EditNoteRequestModel
        {
            Id = request.Id,
            EditedNote = request.EditedNote,
        };
    }

    public NoteModel MapToDomain(Guid id, Guid groupId ,LlmResponseModel llmResponseModel, int pageNumber)
    {
        return new NoteModel
        {
            Id = id,
            DocumentGroupId = groupId,
            Note = llmResponseModel.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text,
            CreatedAt = llmResponseModel.CreatedAt,
            UpdatedAt = llmResponseModel.UpdatedAt,
            PageReference = pageNumber
        };
    }

    public WriteImageNoteRequestModel MapToDomain(WriteImageNoteRequest request)
    {
        return new WriteImageNoteRequestModel
        {
            DocumentGroupId = request.DocumentGroupId,
            Page = request.Page,
            ImageReference = request.ImageReference
        };
    }

    public NoteModel MapToDomain(Note note)
    {
        return new NoteModel
        {
            Id = note.Id,
            DocumentGroupId = note.DocumentGroupId,
            Note = note.Text,
            PageReference = note.RelatedPage,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public List<NoteModel> Map(List<Note> notes)
    {
        return notes.Select(MapToDomain).ToList();
    } 

    public PagedResponseModel<NoteModel>? Map(PagedResponse<Note> response, int page, int size)
    {
        return new PagedResponseModel<NoteModel>
        {
            Items = response.Data.Select(MapToDomain).ToArray(),
            Pagination = new PaginationModel
            {
                Page = page,
                Size = size,
                TotalPages = response.TotalPages,
            }
        };
    }

    public NoteModel MapToDomain(LlmResponseModel llmResponseModel, PageModel page)
    {
        return new NoteModel
        {
            DocumentGroupId = page.DocumentGroupId,
            Note = llmResponseModel.ExtractResponse(), 
            PageReference = page.PageNumber,
            CreatedAt = llmResponseModel.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };
    }
}