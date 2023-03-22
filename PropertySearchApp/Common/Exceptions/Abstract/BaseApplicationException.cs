namespace PropertySearchApp.Common.Exceptions.Abstract;

public class BaseApplicationException : Exception
{
    public string[] Errors { get; }

    public BaseApplicationException(string[] errors)
    {
        Errors = errors;
    }
}