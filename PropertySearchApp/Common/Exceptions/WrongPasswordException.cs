using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class WrongPasswordException : Abstract.HandledApplicationException
{
    public WrongPasswordException(string[] errors) : base(errors)
    { }
}
