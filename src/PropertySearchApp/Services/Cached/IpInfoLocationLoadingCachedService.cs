using Microsoft.Extensions.Caching.Memory;
using PropertySearchApp.Domain;
using PropertySearchApp.Services.Abstract;
using System.Net;

namespace PropertySearchApp.Services.Cached
{
    public class IpInfoLocationLoadingCachedService : ILocationLoadingService
    {
        private readonly ILocationLoadingService _locationLoadingService;
        private readonly IMemoryCache _memoryCache;
        public IpInfoLocationLoadingCachedService(IMemoryCache memoryCache, ILocationLoadingService locationLoadingService)
        {
            _memoryCache = memoryCache;
            _locationLoadingService = locationLoadingService;
        }

        public async Task<LocationDomain> GetLocationByUrlAsync(IPAddress ipAddress, CancellationToken cancellationToken)
        {
            _memoryCache.TryGetValue(ipAddress, out var locationDomain);
            var location = locationDomain as LocationDomain;
            if (location is null)
            {
                location = await _locationLoadingService.GetLocationByUrlAsync(ipAddress, cancellationToken);

                if (string.IsNullOrEmpty(location.Country) == false)
                {
                    _memoryCache.Set(ipAddress, location, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(60),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600)
                    });
                }
            }

            return location;
        }
    }
}
