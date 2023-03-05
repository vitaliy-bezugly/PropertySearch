namespace PropertySearchApp.Common.Exceptions;

public class LoginOperationException : Exception
{
    public string[] Errors { get; set; }

    public LoginOperationException(string[] errors) : base()
    {
        Errors = errors;
    }
}
