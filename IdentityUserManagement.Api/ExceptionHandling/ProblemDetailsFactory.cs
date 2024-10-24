using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Core.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.ExceptionHandling;

public class ProblemDetailsFactory(ILogger<ProblemDetailsFactory> logger) : IProblemDetailsFactory
{
    public ProblemDetails Create(HttpContext httpContext, DomainException domainException)
        => CreateProblemDetails(httpContext, domainException);

    public Results<BadRequest<ProblemDetails>, Conflict<ProblemDetails>, IResult> MapErrorResponse(BaseDomainResponse baseDomainResponse)
    {
        if (baseDomainResponse.ErrorType is null)
        {
            throw new ArgumentNullException(nameof(baseDomainResponse.ErrorType));
        }

        if (baseDomainResponse.Errors is null || baseDomainResponse.Errors.Count == 0)
        {
            return MapBasicErrorResponse(baseDomainResponse.ErrorType);
        }

        var domainException = new DomainException("Error");
        baseDomainResponse.Errors?.ForEach(error => domainException.AddDomainError(error.Code, error.Message));

        var problemDetails = CreateProblemDetails(domainException, "Error", GetProblemStatus(baseDomainResponse.ErrorType.Value));

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

    private ProblemDetails CreateProblemDetails(HttpContext httpContext, DomainException domainException)
    {
        var (statusCode, title) = domainException.ExceptionType switch
        {
            DomainExceptionType.ValidationError => (StatusCodes.Status400BadRequest, "Validation Error"),
            DomainExceptionType.Error or DomainExceptionType.Conflict => (StatusCodes.Status409Conflict, "Error"),
            _ => throw new ArgumentOutOfRangeException()
        };

        httpContext.Response.StatusCode = statusCode;

        return CreateProblemDetails(domainException, title, statusCode);
    }

    private ProblemDetails CreateProblemDetails(DomainException domainException, string title, int statusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = GetProblemType(statusCode),
            Extensions = new Dictionary<string, object?>
            {
                { "errors", domainException.DomainErrors.Select(e => new Error(e.ErrorCode, title, e.ErrorMessage, null)).ToList() }
            }
        };

        return problemDetails;
    }

    private Results<BadRequest<ProblemDetails>, Conflict<ProblemDetails>, IResult> MapBasicErrorResponse(BaseDomainErrorType? errorType)
    {
        return errorType switch
        {
            BaseDomainErrorType.NotFound => TypedResults.NotFound(),
            BaseDomainErrorType.Unknown or BaseDomainErrorType.Conflict => TypedResults.Conflict(),
            BaseDomainErrorType.BadRequest => TypedResults.BadRequest(),
            BaseDomainErrorType.Unauthorized => TypedResults.Unauthorized(),
            _ => throw new NotSupportedException("Error type not supported")
        };
    }

    private string GetProblemType(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            StatusCodes.Status403Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
            StatusCodes.Status404NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            StatusCodes.Status409Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            StatusCodes.Status500InternalServerError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            _ => string.Empty
        };
    }

    private int GetProblemStatus(BaseDomainErrorType errorType)
    {
        return errorType switch
        {
            BaseDomainErrorType.NotFound => StatusCodes.Status404NotFound,
            BaseDomainErrorType.Unknown or BaseDomainErrorType.Conflict => StatusCodes.Status409Conflict,
            BaseDomainErrorType.BadRequest => StatusCodes.Status400BadRequest,
            BaseDomainErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => throw new NotSupportedException("Error type not supported")
        };
    }
}