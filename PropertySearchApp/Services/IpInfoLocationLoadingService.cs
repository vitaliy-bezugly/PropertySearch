using System.Net;
using IPinfo;
using IPinfo.Models;
using Newtonsoft.Json;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.ExternalAPIs;
using PropertySearchApp.Services.Abstract;

namespace PropertySearchApp.Services;

public class IpInfoLocationLoadingService : ILocationLoadingService
{
    private readonly IPinfoClient _client;
    private readonly ILogger<IpInfoLocationLoadingService> _logger;
    public IpInfoLocationLoadingService(IPInfoClientContainer container, ILogger<IpInfoLocationLoadingService> logger)
    {
        _client = container.Client;
        _logger = logger;
    }

    public async Task<LocationDomain> GetLocationByUrlAsync(IPAddress ipAddress, CancellationToken cancellationToken)
    {
        // if ipAddress is local
        if (ipAddress.ToString() == "::1")
        {
            ipAddress = await GetPublicIpAddressAsync(cancellationToken);
        }

        if(ipAddress.Equals(IPAddress.Any))
        {
            return new LocationDomain();
        }

        _logger.LogInformation("Client ip address: " + ipAddress.ToString());
        // making API call
        IPResponse apiResponse = await _client.IPApi.GetDetailsAsync(ipAddress, cancellationToken);

        return new LocationDomain
        {
            Id = Guid.Empty,
            Country = apiResponse.CountryName,
            Region = apiResponse.Region,
            City = apiResponse.City,
            Address = string.Empty
        };
    }

    private async Task<IPAddress> GetPublicIpAddressAsync(CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            httpClient.DefaultRequestHeaders.Add("Keep-Alive", "false");
            httpClient.Timeout = TimeSpan.FromSeconds(3);

            var response = await httpClient.GetAsync("https://api.ipify.org/?format=json", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var ip = JsonConvert.DeserializeObject<IpAddressResponse>(json);

                return IPAddress.Parse(ip.IpAddress);
            }
            
            return IPAddress.Any;
        }
        catch (Exception e)
        {
            if (e is TaskCanceledException)
            {
                return IPAddress.Any;
            }

            throw;
        }
    }
}