using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.User;
using Papyrus.Domain.Services.Interfaces.User;
using Papyrus.Perstistance.Interfaces.Writer;


namespace Papyrus.Domain.Services.User;

public sealed class UserWriterService : IUserWriterService
{
    private readonly IUserWriter _userWriter;
    private readonly IMapper _mapper;


    public UserWriterService(IUserWriter userWriter, IMapper mapper)
    {
        _userWriter = userWriter;
        _mapper = mapper;
    }
    
    public async Task<UserModel> CreateAsync(CreateUserRequestModel request, CancellationToken cancellationToken)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new InvalidOperationException("Password is empty after attempting to hash");
        }
        
        var user = _mapper.MapToPersistence(request.Username, request.Email, hashedPassword, request.Name);
        
        await _userWriter.InsertAsync(user, cancellationToken);

        return _mapper.MapToDomain(user);
    }
}