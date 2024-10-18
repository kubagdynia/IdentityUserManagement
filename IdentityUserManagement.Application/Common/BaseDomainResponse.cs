namespace IdentityUserManagement.Application.Common;

public class BaseDomainResponse
{
    public BaseDomainErrorType? ErrorType { get; set; }
    
    public List<string>? Errors { get; set; }
    
    public bool IsSuccess => ErrorType is null;

    public void AddError(string errorMessage, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType = errorType;
        
        Errors ??= [];
        Errors.Add(errorMessage);
    }
    
    public void AddErrors(IEnumerable<string> errorMessages, BaseDomainErrorType errorType = BaseDomainErrorType.Unknown)
    {
        ErrorType = errorType;
        
        Errors ??= [];
        Errors.AddRange(errorMessages);
    }
}