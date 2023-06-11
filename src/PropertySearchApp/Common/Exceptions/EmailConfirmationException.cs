namespace PropertySearchApp.Common.Exceptions;

public class EmailConfirmationException : Exception
{
    public EmailConfirmationException() : base("User with given id does not exist in database")
    { }
    
    public EmailConfirmationException(string message) : base(message)
    { }

    public EmailConfirmationException(string message, Exception e) : base(message, e)
    { }
}