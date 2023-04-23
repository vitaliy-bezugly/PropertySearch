using System.Net;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Services.Abstract;

public interface ILocationLoadingService
{
    Task<LocationDomain> GetLocationByUrlAsync(IPAddress ipAddress, CancellationToken cancellationToken);
}