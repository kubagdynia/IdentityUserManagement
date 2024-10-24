using IdentityUserManagement.Application.Common;
using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Application.Extensions;

public static class IdentityResultExtensions
{
    public static T MapErrors<T>(this IdentityResult identityResult, BaseDomainErrorType errorType) where T : BaseDomainResponse, new()
    {
        return new T
        {
            Errors = identityResult.Errors.Select(e => new BaseDomainError { Code = e.Code, Message = e.Description }).ToList(),
            ErrorType = errorType
        };
    }
}