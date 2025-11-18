namespace Papyrus.Ai.Handlers.Endpoints.Explanation;

internal static class ExplanationEndpointMapper
{
    internal static void MapExplanationEndpointMappers(this WebApplication app)
    {
        var explanationEndpointGroup = app.MapGroup("explanations")
            .WithTags("Explanation")
            .RequireAuthorization();
        
        explanationEndpointGroup.MapToExplanationWriterEndpoints();
    }
}