namespace PropertySearchApp.Common.Exceptions.Abstract;

public class BaseApplicationException : Exception
{
    public string[] Errors { get; }

    public BaseApplicationException() : base()
    { }
    public BaseApplicationException(string message) : base(message)
    {
        
    }
    public BaseApplicationException(string[] errors) : base(errors[0])
    {
        Errors = errors;
    }
}