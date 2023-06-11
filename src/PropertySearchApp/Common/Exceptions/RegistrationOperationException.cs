using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class RegistrationOperationException : AuthorizationOperationException
{
    public RegistrationOperationException(string[] errors) : base(errors)
    { }
}