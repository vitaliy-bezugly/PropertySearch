namespace PropertySearchApp.Persistence.Exceptions;

public class RoleCreationException : Exception
{
    public IEnumerable<string> Errors { get; set; }
    public RoleCreationException(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}