using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class UserValidationException : Abstract.HandledApplicationException
{
    public UserValidationException() : base()
    {
        
    }
    public UserValidationException(string message) : base(message)
    {
        
    }
    public UserValidationException(string[] errors) : base(errors)
    { }
}