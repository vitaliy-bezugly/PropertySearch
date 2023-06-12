using IPinfo;
using PropertySearch.Api.Services.Abstract;

namespace PropertySearch.Api.Services;

public class DefaultIpInfoClientContainer : IPInfoClientContainer
{
    public DefaultIpInfoClientContainer(IpInfoClientBuilder builder)
    {
        Client = builder.Build();
    }

    public IPinfoClient Client { get; init; }
}