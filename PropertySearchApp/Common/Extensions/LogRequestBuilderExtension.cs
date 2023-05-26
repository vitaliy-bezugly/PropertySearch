using PropertySearchApp.Common.Logging;

namespace PropertySearchApp.Common.Extensions;

public static class LogRequestBuilderExtension
{
    public static LogRequest WithPath(this LogRequest logEntry, string path)
    {
        logEntry.Path = path;
        return logEntry;
    }
    
    public static LogRequest WithMethod(this LogRequest logEntry, string method)
    {
        logEntry.Method = method;
        return logEntry;
    }
    
    public static LogRequest WithAuthenticatedUser(this LogRequest logEntry, string? username)
    {
        logEntry.Username = username;
        return logEntry;
    }
    
    public static LogRequest WithUnauthenticatedUser(this LogRequest logEntry)
    {
        logEntry.Username = "Unauthenticated";
        return logEntry;
    }
    
    public static LogRequest WithStatusCode(this LogRequest logEntry, int statusCode)
    {
        logEntry.StatusCode = statusCode;
        return logEntry;
    }
    
    public static LogRequest WithExecutionTime(this LogRequest logEntry, long executionTimeInMs)
    {
        logEntry.ExecutionTimeInMs = executionTimeInMs;
        return logEntry;
    }
}