using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class UserUpdateOperationException : BaseApplicationException
{
    public UserUpdateOperationException() : base()
    { }
    public UserUpdateOperationException(string message) : base(message)
    { }
    public UserUpdateOperationException(string[] errors) : base(errors)
    { }
}
