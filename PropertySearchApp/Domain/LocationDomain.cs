using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Domain;

public class LocationDomain : DomainBase, IMapFrom<LocationEntity>, IMapFrom<LocationViewModel>
{
    public LocationDomain() : base()
    {
        Country = Region = City = Address = String.Empty;
    }
    
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
}
