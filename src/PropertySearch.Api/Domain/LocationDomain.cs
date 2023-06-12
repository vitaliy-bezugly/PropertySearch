using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain.Abstract;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Models.Location;

namespace PropertySearch.Api.Domain;

public class LocationDomain : DomainBase, IMapFrom<LocationViewModel>, IMapFrom<LocationEntity>
{
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string Address { get; set; }

    public LocationDomain()
    {
        Country = Region = City = Address = string.Empty;
    }
}
