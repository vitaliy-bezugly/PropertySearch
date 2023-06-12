using PropertySearch.Api.Common.Exceptions.Abstract;

namespace PropertySearch.Api.Common.Exceptions;

public class RegistrationOperationException : AuthorizationOperationException
{
    public RegistrationOperationException(string[] errors) : base(errors)
    { }
}