using IPinfo;
using Microsoft.Extensions.Options;
using PropertySearch.Api.Common.Options;
using PropertySearch.Api.Services.Abstract;

namespace PropertySearch.Api.Services;

public class IpInfoClientContainer : IPInfoClientContainer
{
    public IpInfoClientContainer(IOptions<IpInfoOptions> options)
    {
        Client = Build(options);
    }

    public IPinfoClient Client { get; init; }
    
    private IPinfoClient Build(IOptions<IpInfoOptions> options)
    {
        if (string.IsNullOrEmpty(options.Value.IpInfoToken))
        {
            throw new Exception("IpInfoToken is empty. Check your secrets");
        }
        
        string token = options.Value.IpInfoToken;
        return new IPinfoClient.Builder().AccessToken(token).Build();
    }
}