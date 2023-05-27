using PropertySearchApp.Common.Logging;

namespace PropertySearchApp.Common.Extensions;

public static class LogEntryBuilder
{
    public static LogEntry WithClass(this LogEntry logEntry, string className)
    {
        logEntry.Class = className;
        return logEntry;
    }
    public static LogEntry WithMethod(this LogEntry logEntry, string method)
    {
        logEntry.Method = method;
        return logEntry;
    }
    public static LogEntry WithComment(this LogEntry logEntry, string comment)
    {
        logEntry.Comment = comment;
        return logEntry;
    }
    
    public static LogEntry WithOperation(this LogEntry logEntry, string operation)
    {
        logEntry.Operation = operation;
        return logEntry;
    }

    public static LogEntry WithUnknownOperation(this LogEntry logEntry)
    {
        logEntry.Operation = "Unknown";
        return logEntry;
    }
    
    public static LogEntry WithParameter(this LogEntry logEntry, string parameterType, string parameterName, string parameterValue)
    {
        logEntry.Parameters += $"Type: {parameterType}, Name: {parameterName}, Value: {parameterValue}; ";
        return logEntry;
    }
    public static LogEntry WithNoParameters(this LogEntry logEntry)
    {
        logEntry.Parameters = "None";
        return logEntry;
    }
}