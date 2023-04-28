using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class UserNotFoundException : Abstract.HandledApplicationException
{
    public UserNotFoundException() : base("User with given id not found")
    {

    }
    public UserNotFoundException(string[] errors) : base(errors)
    {
    }
}