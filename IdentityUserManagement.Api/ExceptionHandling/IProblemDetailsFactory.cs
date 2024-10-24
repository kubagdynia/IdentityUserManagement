using IdentityUserManagement.Application.Common;
using IdentityUserManagement.Core.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUserManagement.Api.ExceptionHandling;

public interface IProblemDetailsFactory
{
    ProblemDetails Create(HttpContext httpContext, DomainException domainException);
    Results<BadRequest<ProblemDetails>, Conflict<ProblemDetails>, IResult> MapErrorResponse(BaseDomainResponse baseDomainResponse);
}