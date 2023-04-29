using Newtonsoft.Json;

namespace PropertySearchApp.Models.ExternalAPIs;

public class IpAddressResponse
{
    [JsonProperty(PropertyName = "ip")]
    public string IpAddress { get; set; }

    public IpAddressResponse()
    {
        IpAddress = string.Empty;
    }
}