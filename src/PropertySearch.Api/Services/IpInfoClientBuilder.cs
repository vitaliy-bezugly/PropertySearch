using IPinfo;
using Microsoft.Extensions.Options;
using PropertySearch.Api.Common.Options;

namespace PropertySearch.Api.Services;

public class IpInfoClientBuilder
{
    private readonly IOptions<IpInfoOptions> _options;
    public IpInfoClientBuilder(IOptions<IpInfoOptions> options)
    {
        _options = options;
    }

    public IPinfoClient Build()
    {
        if (string.IsNullOrEmpty(_options.Value.IpInfoToken))
        {
            throw new Exception("IpInfoToken is empty. Check your secrets");
        }
        
        string token = _options.Value.IpInfoToken;
        return new IPinfoClient.Builder().AccessToken(token).Build();
    }
}