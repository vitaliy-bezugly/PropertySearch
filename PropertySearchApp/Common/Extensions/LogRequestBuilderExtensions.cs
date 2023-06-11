using PropertySearchApp.Common.Logging;

namespace PropertySearchApp.Common.Extensions;

public static class LogRequestBuilderExtensions
{
    public static RequestLog WithPath(this RequestLog logEntry, string path)
    {
        logEntry.Path = path;
        return logEntry;
    }
    
    public static RequestLog WithMethod(this RequestLog logEntry, string method)
    {
        logEntry.Method = method;
        return logEntry;
    }
    
    public static RequestLog WithAuthenticatedUser(this RequestLog logEntry, string? username)
    {
        logEntry.Username = username;
        return logEntry;
    }
    
    public static RequestLog WithUnauthenticatedUser(this RequestLog logEntry)
    {
        logEntry.Username = null;
        return logEntry;
    }
    
    public static RequestLog WithStatusCode(this RequestLog logEntry, int statusCode)
    {
        logEntry.StatusCode = statusCode;
        return logEntry;
    }
    
    public static RequestLog WithExecutionTime(this RequestLog logEntry, long executionTimeInMs)
    {
        logEntry.ExecutionTimeInMs = executionTimeInMs;
        return logEntry;
    }
}