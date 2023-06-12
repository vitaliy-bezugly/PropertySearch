using Newtonsoft.Json;

namespace PropertySearch.Api.Common.Logging;

public class RequestLog
{
    public string Path { get; set; } = String.Empty;
    public string Method { get; set; } = String.Empty;
    public string? Username { get; set; }
    public int StatusCode { get; set; }
    public long ExecutionTimeInMs { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}