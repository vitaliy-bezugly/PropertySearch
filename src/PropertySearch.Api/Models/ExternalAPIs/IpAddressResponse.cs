using Newtonsoft.Json;

namespace PropertySearch.Api.Models.ExternalAPIs;

public class IpAddressResponse
{
    [JsonProperty(PropertyName = "ip")]
    public string IpAddress { get; set; }

    public IpAddressResponse()
    {
        IpAddress = string.Empty;
    }
}