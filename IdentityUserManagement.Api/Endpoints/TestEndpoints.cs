using IdentityUserManagement.Core.Constants;

namespace IdentityUserManagement.Api.Endpoints;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("api/test");

        GetTest(route);

        return app;
    }
    
    private static void GetTest(RouteGroupBuilder route)
    {
        route.MapGet("",
                async Task<IResult>(string? name) =>
                {
                    name ??= "World";
                    await Task.CompletedTask;
                    return TypedResults.Ok($"Hello, {name}!");
                })
            .WithName("Test")
            .WithSummary("Test endpoint")
            .RequireAuthorization(UserRoles.Admin)
            .Produces<string>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
    }
}