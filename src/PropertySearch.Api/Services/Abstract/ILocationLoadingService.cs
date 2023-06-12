using System.Net;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Services.Abstract;

public interface ILocationLoadingService
{
    Task<LocationDomain> GetLocationByUrlAsync(IPAddress ipAddress, CancellationToken cancellationToken);
}