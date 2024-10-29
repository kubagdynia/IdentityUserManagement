namespace IdentityUserManagement.Application.Common;

public class BaseDomainResponse
{
    public BaseDomainErrorType? ErrorType { get; set; }
    
    public List<BaseDomainError>? Errors { get; set; }
    
    public bool IsSuccess => ErrorType is null;
}

public static class BaseDomainResponseExtensions
{
    public static T AddError<T>(this T response, string code, string message, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown) where T : BaseDomainResponse
    {
        response.ErrorType ??= errorType;
        response.Errors ??= [];
        response.Errors.Add(new BaseDomainError { Code = code, Message = message });
        return response;
    }
    
    public static T AddErrors<T>(this T response, IEnumerable<BaseDomainError> errorMessages, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown) where T : BaseDomainResponse
    {
        response.ErrorType ??= errorType;
        response.Errors ??= [];
        response.Errors.AddRange(errorMessages);
        return response;
    }
}