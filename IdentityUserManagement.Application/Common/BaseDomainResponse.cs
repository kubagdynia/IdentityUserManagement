namespace IdentityUserManagement.Application.Common;

public class BaseDomainResponse
{
    public BaseDomainErrorType? ErrorType { get; set; }
    
    public List<BaseDomainError>? Errors { get; set; }
    
    public bool IsSuccess => ErrorType is null;

    public void AddError(string code, string message, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType ??= errorType;
        Errors ??= [];
        Errors.Add(new BaseDomainError { Code = code, Message = message });
    }
    
    public void AddErrors(IEnumerable<BaseDomainError> errorMessages, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType ??= errorType;
        
        Errors ??= [];
        Errors.AddRange(errorMessages);
    }
}