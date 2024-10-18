using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Api.Endpoints.Mappers;
using IdentityUserManagement.Api.ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("api/accounts");
        
        // POST /api/accounts/register
        RegisterUser(route);
        
        // POST /api/accounts/authenticate
        AuthenticateUser(route);
        
        return app;
    }

    // Register a new user
    private static void RegisterUser(RouteGroupBuilder route)
    {
        route.MapPost("/register",
                async Task<IResult> (RegisterUserRequest request, IMediator mediator) =>
                {
                    var result = await mediator.Send(request.ToCommand());
                    return !result.IsSuccess ? ProblemDetailsFactory.MapErrorResponse(result) : TypedResults.Created();
                })
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
    
    private static void AuthenticateUser(RouteGroupBuilder route)
    {
        route.MapPost("/authenticate",
                async Task<IResult>
                    (AuthenticateUserRequest request, IMediator mediator) =>
                {
                    var result = await mediator.Send(request.ToCommand());
                    return !result.IsSuccess
                        ? ProblemDetailsFactory.MapErrorResponse(result)
                        : TypedResults.Ok(result.ToResponse());
                })
            .WithName("AuthenticateUser")
            .WithSummary("Authenticate a user")
            .Produces<AuthenticateUserResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
    }
}