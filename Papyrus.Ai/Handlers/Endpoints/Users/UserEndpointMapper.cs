namespace Papyrus.Ai.Handlers.Endpoints.Users;

internal static class UserEndpointMapper
{
    internal static void MapUserEndpoints(this WebApplication app)
    {
        var userEndpoints = app.MapGroup("user")
            .WithTags("Users");
        
        userEndpoints.MapUsersEndpoint();
    }
}