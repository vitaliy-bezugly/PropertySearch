using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class LoginOperationException : AuthorizationOperationException
{
    public LoginOperationException(string[] errors) : base(errors)
    { }
}
