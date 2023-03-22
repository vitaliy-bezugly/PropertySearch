namespace PropertySearchApp.Common.Exceptions;

public class AccommodationDataSourceException : Exception
{
    public string[] Errors { get; }
    public AccommodationDataSourceException(string[] errors) : base()
    {
        Errors = errors;
    }
}