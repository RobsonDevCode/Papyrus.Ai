using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Papyrus.Ai.Constants;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Services.Interfaces.Authentication;
using Papyrus.Domain.Services.Interfaces.User;
using static Papyrus.Ai.Constants.LoggingCategories;

namespace Papyrus.Ai.Handlers.Endpoints.Users;

internal static class UserReaderEndpoints
{
    internal static void MapUserReaderEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("login", Login);
    }


    private static async Task<Results<Ok<UserResponse>, BadRequest<string>>> Login(
        [FromBody] LoginRequest request,
        [FromServices] IUserReaderService userReaderService,
        [FromServices] IJwtService jwtService,
        [FromServices] IValidator<LoginRequest> loginRequestValidator,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.UserReader);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Login Request",
            [Email] = request.Email
        });

        var validator = await loginRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validator.IsValid)
        {
            var errors = string.Join(" | ", validator.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }

        logger.LogInformation("Attempting to Login");
        var user = await userReaderService.LoginAsync( request.Email, request.Password,
            cancellationToken);

        var authToken = jwtService.GenerateJwtToken(user.Id, user.Username, user.Email, ["User"]);
        
        httpContext.AddJwt(authToken.JWT, authToken.Expires);

        return TypedResults.Ok(mapper.MapToResponse(user));
    }
}