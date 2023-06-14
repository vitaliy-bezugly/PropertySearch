using System.Net;
using IPinfo;
using IPinfo.Models;
using Newtonsoft.Json;
using PropertySearch.Api.Common.Logging;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Services.Abstract;
using PropertySearch.Api.Common.Extensions;
using PropertySearch.Api.Models.ExternalAPIs;

namespace PropertySearch.Api.Services;

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
        try
        {
            if (ipAddress.ToString() == "::1" || ipAddress.Equals(IPAddress.Any))
            {
                return new LocationDomain();
            }

            _logger.LogInformation("Client ip address: " + ipAddress);
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
        catch (Exception e)
        {
            _logger.LogError(new LogEntry()
                .WithClass(nameof(IpInfoLocationLoadingService))
                .WithMethod(nameof(GetLocationByUrlAsync))
                .WithOperation("Get")
                .WithComment(e.Message)
                .WithParameter(typeof(IPAddress).FullName ?? String.Empty, nameof(ipAddress), ipAddress.ToString())
                .ToString());

            throw;
        }
    }
}