using IPinfo;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class DefaultIpInfoClientContainer : IPInfoClientContainer
{
    public DefaultIpInfoClientContainer(IpInfoClientBuilder builder)
    {
        Client = builder.Build();
    }

    public IPinfoClient Client { get; init; }
}