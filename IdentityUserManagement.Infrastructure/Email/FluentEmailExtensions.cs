using FluentEmail.Core.Interfaces;
using IdentityUserManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdentityUserManagement.Infrastructure.Email;

public static class FluentEmailExtensions
{
    public static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration,
        string configSectionName = "EmailSettings")
    {
        var emailSettings =
            configuration.GetSection(configSectionName).Get<EmailSettings>() ?? GetDefaultEmailSettings();
        
        return services.AddFluentEmail(emailSettings);
    }
    
    public static IServiceCollection AddFluentEmail(this IServiceCollection services, EmailSettings emailSettings)
    {
        ArgumentNullException.ThrowIfNull(emailSettings, nameof(emailSettings));
        
        var fluentEmailBuilder =
            services.AddFluentEmail(emailSettings.DefaultFromEmail, emailSettings.DefaultFromDisplayName)
                .AddRazorRenderer();

        if (emailSettings.SaveToFile.Enabled)
        {
            fluentEmailBuilder.AddSaveToFileSender(emailSettings.SaveToFile.Path);
        }
        else
        {
            AddSmtpSender(fluentEmailBuilder, emailSettings.SmtpSettings);
        }
        
        services.TryAddScoped<IEmailService, EmailService>();
        
        return services;
    }

    private static void AddSaveToFileSender(this FluentEmailServicesBuilder builder, string path)
        => builder.Services.TryAdd(ServiceDescriptor.Scoped<ISender>(_ => new EmailToFileSender(path)));
    
    private static void AddSmtpSender(FluentEmailServicesBuilder builder, SmtpSettings smtpSettings)
    {
        if (string.IsNullOrEmpty(smtpSettings.UserName) || string.IsNullOrEmpty(smtpSettings.Password))
        {
            builder.AddSmtpSender(smtpSettings.Host, smtpSettings.Port);
        }
        else
        {
            builder.AddSmtpSender(smtpSettings.Host, smtpSettings.Port, smtpSettings.UserName, smtpSettings.Password);
        }
    }
    
    private static EmailSettings GetDefaultEmailSettings()
        => new(
            DefaultFromEmail: "test@app.com",
            DefaultFromDisplayName: "Test App",
            SaveToFile: new SaveToFile(Enabled: true, Path: "../Emails"),
            SmtpSettings: new SmtpSettings(Host: "smtp.gmail.com", Port: 587)
        );
}

public record EmailSettings(string DefaultFromEmail, string DefaultFromDisplayName, SaveToFile SaveToFile, SmtpSettings SmtpSettings);

public record SaveToFile(bool Enabled, string Path);

public record SmtpSettings(string Host, int Port, string? UserName = null, string? Password = null);