namespace PropertySearchApp.Common.Exceptions;

public class InternalDatabaseException : Exception
{
    public string[] Errors { get; }
    public InternalDatabaseException(string[] errors) : base()
    {
        Errors = errors;
    }
}