using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Papyrus.Domain.Exceptions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Authentication;
using Papyrus.Domain.Services.Interfaces.Authentication;
using Papyrus.Domain.Services.Interfaces.User;
using Papyrus.Perstistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Writer;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Papyrus.Domain.Services.Authentication;

public sealed class JwtService : IJwtService
{
    private readonly IUserReaderService _userReaderService;
    private readonly ITokenReader _tokenReader;
    private readonly ITokenWriter _tokenWriter;
    private readonly IMapper _mapper;
    private readonly ILogger<JwtService> _logger;
    private JwtSettings _settings;

    public JwtService(
        IUserReaderService userReaderService,
        ITokenReader tokenReader,
        ITokenWriter tokenWriter,
        IMapper mapper,
        ILogger<JwtService> logger,
        IOptions<JwtSettings> settings)
    {
        _userReaderService = userReaderService;
        _tokenReader = tokenReader;
        _tokenWriter = tokenWriter;
        _mapper = mapper;
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task<JwtModel> GenerateJwtToken(Guid userId, string username, string email,
        IEnumerable<string>? roles = null, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (roles is not null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours),
            signingCredentials: credentials);

        var refreshToken = GenerateRefreshToken();

        await SaveRefreshTokenAsync(userId, refreshToken, cancellationToken);
        return new JwtModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = token.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public async Task<JwtModel> ReGenerateJwtAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        try
        {

            if (!httpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                _logger.LogWarning("Refresh token was not found");
                throw new UnauthorizedAccessException();
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogError("Refresh token is empty");
                throw new UnauthorizedAccessException();
            }

            var validationResults = await ValidateRefreshTokenAsync(refreshToken, cancellationToken);
            if (!validationResults.IsValid)
            {
                _logger.LogError("Refresh token is invalid");
                throw new UnauthorizedAccessException();
            }

            if (validationResults.Token is null)
            {
                _logger.LogError("Refresh token is null after validation");
                throw new UnauthorizedAccessException();
            }

            var user = await _userReaderService.GetById(validationResults.Token.UserId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException("User not found when validating refresh token.");
            }

            await _tokenWriter.DeleteAsync(refreshToken, cancellationToken);
            var newJwt = await GenerateJwtToken(user.Id, user.Username, user.Email, ["roles"], cancellationToken);

            return newJwt;
        }
        catch
        {
            httpContext.Response.Cookies.Delete("refresh_token");
            throw;
        }
    }
    
    private async Task<(RefreshTokenModel? Token, bool IsValid)> ValidateRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        var token = await _tokenReader.GetAsync(refreshToken, cancellationToken);
        if (token is null || token.ExpiresAt < DateTime.UtcNow || token.IsRevoked)
        {
            await _tokenWriter.DeleteAsync(refreshToken, cancellationToken);
            return (null, false);
        }

        return (_mapper.MapToDomain(token), true);
    }
    
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task SaveRefreshTokenAsync(Guid userId, string refreshToken,
        CancellationToken cancellationToken)
    {
        var tokenToSave = _mapper.MapToPersistence(userId, refreshToken);

        await _tokenWriter.SaveAsync(tokenToSave, cancellationToken);
    }

   

  
}