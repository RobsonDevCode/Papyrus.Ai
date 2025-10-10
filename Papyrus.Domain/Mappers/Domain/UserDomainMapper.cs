using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.User;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public UserModel MapToDomain(User user)
    {
        return new UserModel
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Password = user.PasswordHash,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }

    public CreateUserRequestModel MapToDomain(CreateUserRequest request)
    {
        return new CreateUserRequestModel
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
        };
    }
}