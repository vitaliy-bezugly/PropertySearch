namespace PropertySearchApp.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User with given id does not exist in database")
    { }
    
    public UserNotFoundException(string message) : base(message)
    { }

    public UserNotFoundException(string message, Exception e) : base(message, e)
    { }
}