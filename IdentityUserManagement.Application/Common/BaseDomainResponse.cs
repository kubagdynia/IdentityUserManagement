namespace IdentityUserManagement.Application.Common;

public class BaseDomainResponse
{
    public List<string>? Errors { get; private set; }

    public void AddError(string errorMessage)
    {
        Errors ??= [];
        Errors.Add(errorMessage);
    }
    
    public void AddErrors(IEnumerable<string> errorMessages)
    {
        Errors ??= [];
        Errors.AddRange(errorMessages);
    }
}