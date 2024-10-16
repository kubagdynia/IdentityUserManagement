using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Core.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.ExceptionHandling;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Create(HttpContext httpContext, DomainException domainException)
    {
        // Handle domain exceptions
        var problemDetails = domainException.ExceptionType switch
        {
            DomainExceptionType.ValidationError => ExecuteValidationError(httpContext, domainException),
            DomainExceptionType.Error => ExecuteConflict(httpContext, domainException),
            DomainExceptionType.Conflict => ExecuteConflict(httpContext, domainException),
            _ => throw new ArgumentOutOfRangeException()
        };

        return problemDetails;
    }
    
    public static Results<BadRequest<ProblemDetails>, Conflict<ProblemDetails>, IResult> MapErrorResponse(BaseDomainResponse baseDomainResponse)
    {
        if (baseDomainResponse.ErrorType is null)
        {
            throw new ArgumentNullException(nameof(baseDomainResponse.ErrorType));
        }
        
        if (baseDomainResponse.Errors is null || baseDomainResponse.Errors.Count == 0)
        {
            return baseDomainResponse.ErrorType switch
            {
                BaseDomainErrorType.NotFound => TypedResults.NotFound(),
                BaseDomainErrorType.Unknown => TypedResults.Conflict(),
                BaseDomainErrorType.Conflict => TypedResults.Conflict(),
                BaseDomainErrorType.BadRequest => TypedResults.BadRequest(),
                BaseDomainErrorType.Unauthorized => TypedResults.Unauthorized(),
                _ => throw new NotSupportedException("Error type not supported")
            };
        }
        
        var domainException = new DomainException("Error");
        baseDomainResponse.Errors?.ForEach(error => domainException.AddDomainError("Error", error));
        
        var problemDetails = GetProblemDetails(
            domainException,
            title: "Error",
            problemStatus: StatusCodes.Status400BadRequest,
            detailMessage: null);
        
        return baseDomainResponse.ErrorType switch
        {
            BaseDomainErrorType.NotFound => TypedResults.NotFound(problemDetails),
            BaseDomainErrorType.Unknown => TypedResults.Conflict(problemDetails),
            BaseDomainErrorType.Conflict => TypedResults.Conflict(problemDetails),
            BaseDomainErrorType.BadRequest => TypedResults.BadRequest(problemDetails),
            BaseDomainErrorType.Unauthorized => TypedResults.Unauthorized(),
            _ => throw new NotSupportedException("Error type not supported")
        };
    }
    
    // Execute validation error
    private static ProblemDetails ExecuteValidationError(HttpContext httpContext, DomainException domainException)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = GetProblemDetails(
            domainException,
            title: "Validation Error",
            problemStatus: StatusCodes.Status400BadRequest,
            detailMessage: null);
            //detailMessage: error => $"Validation for '{error.PropertyName}' with value '{error.AttemptedValue}' failed in {error.ClassName}");

        return problemDetails;
    }
    
    // Execute conflict error
    private static ProblemDetails ExecuteConflict(HttpContext httpContext, DomainException domainException)
    {
        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        
        var problemDetails = GetProblemDetails(
            domainException,
            title: "Error",
            problemStatus: StatusCodes.Status409Conflict,
            detailMessage: null);

        return problemDetails;
    }

    // Get problem details from domain exception
    private static ProblemDetails GetProblemDetails(DomainException domainException,
        string title, int problemStatus,
        Func<DomainError, string>? detailMessage = null)
    {
        string problemType = GetProblemType(problemStatus);
        
        var problemDetails = new ProblemDetails
        {
            Status = problemStatus,
            Title = title,
            Type = problemType
        };
        
        var errors = new List<Error>();
        
        // Add domain errors to the problem details
        foreach (var error in domainException.DomainErrors)
        {
            errors.Add(new Error(
                Code: error.ErrorCode,
                Title: title,
                UserMessage: error.ErrorMessage,
                Details: detailMessage?.Invoke(error)
            ));
        }
        
        // Add errors to the problem details extensions
        if (errors.Any())
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                { "errors", errors }
            };
        }
        
        return problemDetails;
    }

    private static string GetProblemType(int problemStatus)
    {
        return problemStatus switch
        {
            StatusCodes.Status400BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            StatusCodes.Status409Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            _ => string.Empty
        };
    }
}