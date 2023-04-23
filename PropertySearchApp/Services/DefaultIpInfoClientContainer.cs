using IPinfo;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class DefaultIpInfoClientContainer : IPInfoClientContainer
{
    private readonly IPinfoClient _client;
    public DefaultIpInfoClientContainer(IpInfoClientBuilder builder)
    {
        _client = builder.Build();
    }

    public IPinfoClient Client
    {
        get => _client;
    }
}