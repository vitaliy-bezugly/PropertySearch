using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class UserValidationException : BaseApplicationException
{
    public UserValidationException(string[] errors) : base(errors)
    { }
}