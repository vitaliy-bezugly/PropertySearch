namespace PropertySearchApp.Persistence.Exceptions;

public class NotFoundRoleException : Exception
{
    public NotFoundRoleException(string message) : base(message)
    {  }
}
