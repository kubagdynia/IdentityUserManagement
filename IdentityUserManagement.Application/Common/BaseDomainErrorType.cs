namespace IdentityUserManagement.Application.Common;

public enum BaseDomainErrorType
{
    Unknown,
    NotFound,
    BadRequest,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError
}