using IdentityUserManagement.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.ExceptionHandling;

public sealed record Error(string? Code, string? Title, string? Details, string? UserMessage);

// Global exception handler is responsible for handling exceptions that occur in the application
// It logs the exception and returns a ProblemDetails object with the appropriate status code and error message based on the exception type
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // Try to handle the exception and return true if the exception was handled
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"Exception occured: {exception.Message}");
        
        if (exception is DomainException domainException)
        {
            // Handle domain exceptions
            var problemDetails = ProblemDetailsFactory.Create(httpContext, domainException);
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
        else
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };
            
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        return true;
    }
}