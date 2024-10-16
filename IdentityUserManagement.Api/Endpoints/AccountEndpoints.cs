using FluentValidation;
using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Application.Mappers;
using IdentityUserManagement.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

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
            async Task<Results<Created, BadRequest, BadRequest<RegistrationResponseDto>>>
                (UserRegistrationDto userRegistration, UserManager<User> userManager,
                    IValidator<UserRegistrationDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(userRegistration);
                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(new RegistrationResponseDto(IsSuccessRegistration: false,
                        Errors: validationResult.Errors.Select(e => e.ErrorMessage)));
                }
                
                User user = userRegistration.MapToUser();
                
                // Save the user to the database
                IdentityResult result = await userManager.CreateAsync(user, userRegistration.Password!);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    return TypedResults.BadRequest(new RegistrationResponseDto(IsSuccessRegistration: false,  Errors: errors));
                }

                return TypedResults.Created();
            })
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .WithOpenApi();
    }
}