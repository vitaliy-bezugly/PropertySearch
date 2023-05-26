using Newtonsoft.Json;

namespace PropertySearchApp.Common.Logging;

public class LogRequest
{
    public string Path { get; set; } = String.Empty;
    public string Method { get; set; } = String.Empty;
    public string? Username { get; set; }
    public int StatusCode { get; set; }
    public long ExecutionTimeInMs { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}