using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class ContactValidationException : Abstract.HandledApplicationException
{
    public ContactValidationException(string error) : base(error)
    { }
}
