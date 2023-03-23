using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class UserNotFoundException : BaseApplicationException
{
    public UserNotFoundException() : base()
    {
        
    }
    public UserNotFoundException(string[] errors) : base(errors)
    {
    }
}