using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class WrongPasswordException : BaseApplicationException
{
    public WrongPasswordException(string[] errors) : base(errors)
    { }
}
