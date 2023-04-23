using IPinfo;

namespace PropertySearchApp.Services;

public class IpInfoClientBuilder
{
    private readonly IConfiguration _configuration;
    public IpInfoClientBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IPinfoClient Build()
    {
        string token = _configuration.GetSection("Tokens").GetSection("IpInfoToken").Value;
        return new IPinfoClient.Builder().AccessToken(token).Build();
    }
}