namespace PropertySearchApp.Common.Exceptions.Abstract;

public class HandledApplicationException : Exception
{
    public string[] Errors { get; }

    public HandledApplicationException() : base()
    {
        Errors = new string[0];
    }
    public HandledApplicationException(string message) : base(message)
    {
        Errors = new string[] { message };
    }
    public HandledApplicationException(string[] errors) : base(errors[0])
    {
        Errors = errors;
    }
}