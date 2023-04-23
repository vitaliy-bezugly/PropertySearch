using IPinfo;

namespace PropertySearchApp.Services;

public class IpClientContainer
{
    private readonly IpClientBuilder _builder;
    private IPinfoClient? _client;
    public IpClientContainer(IpClientBuilder builder)
    {
        _client = null;
        _builder = builder;
    }

    public IPinfoClient Client
    {
        get
        {
            if (_client is null)
                _client = _builder.Build();
            return _client;
        }
    }
}