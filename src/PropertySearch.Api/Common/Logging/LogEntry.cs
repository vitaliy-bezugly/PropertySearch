using Newtonsoft.Json;

namespace PropertySearch.Api.Common.Logging;

public class LogEntry
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string? Operation { get; set; }
    public string Parameters { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}