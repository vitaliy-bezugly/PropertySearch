using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class AccommodationValidationException : Abstract.HandledApplicationException
{
    public AccommodationValidationException() : base()
    { }

    public AccommodationValidationException(string message) : base(message)
    { }
    public AccommodationValidationException(string[] errors) : base(errors)
    { }
}