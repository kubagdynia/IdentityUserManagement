using Microsoft.AspNetCore.Http.HttpResults;

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
                Results<Ok<string>, NotFound, BadRequest>(string? name) =>
                {
                    name ??= "World";
                    return TypedResults.Ok($"Hello, {name}!");
                })
            .WithName("Test")
            .WithSummary("Test endpoint")
            .RequireAuthorization()
            .Produces<Ok<string>>()
            .Produces<NotFound>()
            .Produces<BadRequest>()
            .WithOpenApi();
    }
}