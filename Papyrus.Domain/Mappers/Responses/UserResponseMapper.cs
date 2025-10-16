using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.User;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public UserResponse MapToResponse(UserModel user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}