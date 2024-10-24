namespace IdentityUserManagement.Application.Common;

public class BaseDomainResponse
{
    public BaseDomainErrorType? ErrorType { get; set; }
    
    public List<BaseDomainError>? Errors { get; set; }
    
    public bool IsSuccess => ErrorType is null;

    public void AddError(BaseDomainError errorMessage, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType ??= errorType;
        
        Errors ??= [];
        Errors.Add(errorMessage);
    }
    
    public void AddErrors(IEnumerable<BaseDomainError> errorMessages, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType ??= errorType;
        
        Errors ??= [];
        Errors.AddRange(errorMessages);
    }
}