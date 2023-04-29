namespace PropertySearchApp.Common;

public class OperationResult
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }

    public OperationResult()
    {
        Succeeded = true;
        ErrorMessage = string.Empty;
    }
    public OperationResult(string errorMessage)
    {
        Succeeded = false;
        ErrorMessage = errorMessage;
    }
    public OperationResult(IEnumerable<string> errorMessages)
    {
        Succeeded = false;
        ErrorMessage = string.Empty;
        foreach (var item in errorMessages)
        {
            ErrorMessage += item + ";";
        }
    }
}

public class OperationResult<T> : OperationResult
    where T : class
{
    public T? Value { get; set; }

    public OperationResult(T value) : base()
    {
        Value = value;
    }
    public OperationResult(string errorMessage) : base(errorMessage) 
    {
        Value = default;
    }
    public OperationResult(IEnumerable<string> errorMessages) : base(errorMessages)
    {
        Value = default;
    }
}