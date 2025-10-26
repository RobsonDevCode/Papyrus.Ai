using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Papyrus.Domain.Models.Authentication;

namespace Papyrus.Ai.Extensions;

public static class AuthenticationExtensions
{
    public static void AddPapyrusAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        if (settings == null)
        {
            throw new ApplicationException($"{nameof(JwtSettings)} is has not been configured.");
        }
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
                
            });
        
        services.AddAuthorization();
    }
}