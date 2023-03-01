namespace PropertySearchApp.Common.Exceptions;

public class RegistrationOperationException : Exception
{
    private IEnumerable<string> _errors;

    public RegistrationOperationException(IEnumerable<string> errors) : base()
    {
        _errors = errors;
    }

    public IEnumerable<string> GetErrors()
    {
        return _errors;
    }
}