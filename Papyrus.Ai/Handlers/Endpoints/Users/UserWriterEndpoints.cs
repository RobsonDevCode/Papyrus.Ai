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

internal static class UserWriterEndpoints
{
    internal static void MapUserWriterEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("", Create);
    }

    private static async Task<Results<Created<UserResponse>, BadRequest<string>>> Create(
        [FromBody] CreateUserRequest request,
        [FromServices] IUserWriterService userWriterService,
        [FromServices] IJwtService jwtService,
        [FromServices] IValidator<CreateUserRequest> createUserRequestValidator,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] ILoggerFactory loggerFactory,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(Loggers.UserWriter);
        using var _ = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = $"UpsertAsync User {request.Username}"
        });

        var validator = await createUserRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validator.IsValid)
        {
            var errors = string.Join(" | ", validator.Errors.Select(x => x.ErrorMessage));
            return TypedResults.BadRequest(errors);
        }
        
        logger.LogInformation("Creating new user {username}", request.Username);

        var mappedRequest = mapper.MapToDomain(request);
        
        var response =
            await userWriterService.CreateAsync(mappedRequest, cancellationToken);

        var authToken = jwtService.GenerateJwtToken(response.Id, response.Username, response.Email, ["User"]);
        httpContext.AddJwt(authToken.JWT, authToken.Expires);
        logger.LogInformation(authToken.JWT);
        var baseUrl = configuration.GetValue<string>("PapyrusApiUrl")
            ?? throw new NullReferenceException("PapyrusApiUrl cannot be null");
        
        return TypedResults.Created(baseUrl, new UserResponse
        {
            Id = response.Id,
            Username = response.Username,
            Name = response.Name,
            Email = response.Email,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        });
    }
}