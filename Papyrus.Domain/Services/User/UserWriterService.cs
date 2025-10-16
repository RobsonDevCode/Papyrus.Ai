using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.User;
using Papyrus.Domain.Services.Interfaces.User;
using Papyrus.Perstistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Writer;


namespace Papyrus.Domain.Services.User;

public sealed class UserWriterService : IUserWriterService
{
    private readonly IUserWriter _userWriter;
    private readonly IUserReader _userReader;
    private readonly IMapper _mapper;
    private readonly ILogger<UserWriterService> _logger;
    

    public UserWriterService(IUserWriter userWriter,
        IUserReader userReader,
        IMapper mapper,
        ILogger<UserWriterService> logger)
    {
        _userWriter = userWriter;
        _userReader = userReader;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<UserModel> CreateAsync(CreateUserRequestModel request, CancellationToken cancellationToken)
    {
        if (await _userReader.ExistsAsync(request.Username, request.Email, cancellationToken))
        {
            _logger.LogWarning("User {username} already exists.", request.Username);
            throw new BadHttpRequestException("User already exists");
        }        
        
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