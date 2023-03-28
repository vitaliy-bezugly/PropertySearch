namespace PropertySearchApp.Common.Exceptions.Abstract;

public class BaseApplicationException : Exception
{
    public string[] Errors { get; }

    public BaseApplicationException() : base()
    {
        Errors = new string[0];
    }
    public BaseApplicationException(string message) : base(message)
    {
        Errors = new string[] { message };
    }
    public BaseApplicationException(string[] errors) : base(errors[0])
    {
        Errors = errors;
    }
}