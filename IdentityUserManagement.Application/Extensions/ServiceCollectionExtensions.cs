using FluentValidation;
using IdentityUserManagement.Application.Behaviors;
using MediatR.NotificationPublishers;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityUserManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        // Register MediatR and behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestPostProcessorBehavior<,>));
            // TaskWhenAllPublisher means that notifications will be published in parallel, and this mechanism
            // will wait for all tasks to complete before proceeding with further operations.
            // This approach ensures that all notification handlers are executed in parallel, which can reduce
            // the overall execution time, especially when handlers perform independent asynchronous operations.
            // Note! If you want to publish notifications in sequence, you can use the NoOpPublisher.
            cfg.NotificationPublisher = new TaskWhenAllPublisher(); // Default is new NoOpPublisher();
        });
        
        // Register validators if specified
        ValidatorOptions.Global.LanguageManager.Enabled = false; // Disable the language manager to prevent the default language manager from being registered.
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
    
}