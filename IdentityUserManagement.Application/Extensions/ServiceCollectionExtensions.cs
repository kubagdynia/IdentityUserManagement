using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityUserManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        // Register validators if specified
        ValidatorOptions.Global.LanguageManager.Enabled = false; // Disable the language manager to prevent the default language manager from being registered.
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
    
}