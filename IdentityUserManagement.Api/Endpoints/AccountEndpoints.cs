using IdentityUserManagement.Api.Endpoints.Contracts.V1;
using IdentityUserManagement.Api.Endpoints.Mappers;
using IdentityUserManagement.Api.ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("api/accounts");
        
        // POST /api/accounts/register
        RegisterUser(route);
        
        return app;
    }

    // Register a new user
    private static void RegisterUser(RouteGroupBuilder route)
    {
        route.MapPost("/register",
                async Task<Results<Created, BadRequest, BadRequest<ProblemDetails>>> (RegisterUserRequest request, IMediator mediator) =>
                {
                    var result = await mediator.Send(request.ToCommand());
                    if (!result.IsSuccessRegistration)
                    {
                        return ProblemDetailsFactory.CreateBadRequest(result.Errors);
                    }
                    
                    return TypedResults.Created();
                })
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .WithOpenApi();
    }
}