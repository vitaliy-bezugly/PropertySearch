using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class AccommodationValidationException : BaseApplicationException
{
    public AccommodationValidationException(string[] errors) : base(errors)
    { }
}