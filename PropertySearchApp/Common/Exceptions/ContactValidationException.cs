using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class ContactValidationException : BaseApplicationException
{
    public ContactValidationException(string error) : base(error)
    { }
}
