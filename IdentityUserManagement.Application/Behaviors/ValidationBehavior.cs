using FluentValidation;
using FluentValidation.Results;
using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Core.Exceptions;
using MediatR;

namespace IdentityUserManagement.Application.Behaviors;

// Validation behavior for MediatR requests
// This behavior validates the request using FluentValidation
// If there are errors, a domain exception is thrown
// The domain exception contains the validation errors
// The validation errors are added as domain errors
// The domain errors are mapped from the FluentValidation validation failures
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseDomainResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next().ConfigureAwait(false);
        
        var context = new ValidationContext<TRequest>(request);

        // Validate the request
        IEnumerable<ValidationFailure> errors = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        // If there are errors, throw a validation exception
        if (errors.Any())
        {
            ThrowValidationException(errors);
        }

        return await next().ConfigureAwait(false);
    }
    
    private static void ThrowValidationException(IEnumerable<ValidationFailure> errors)
    {
        // Create a domain exception
        var exception = new DomainException("Validation Error", DomainExceptionType.ValidationError);

        // Add domain errors
        foreach (var error in errors)
        {
            exception.AddDomainError(error.ErrorCode, error.ErrorMessage, error.PropertyName, error.AttemptedValue,
                typeof(TRequest).Name, GetErrorType(error.Severity));
        }

        throw exception;
    }
    
    private static DomainErrorType GetErrorType(Severity severity)
    {
        // Map severity to domain error type
        var errorType = severity switch
        {
            Severity.Error => DomainErrorType.Error,
            Severity.Warning => DomainErrorType.Warning,
            Severity.Info => DomainErrorType.Info,
            _ => DomainErrorType.Error
        };
        return errorType;
    }
}