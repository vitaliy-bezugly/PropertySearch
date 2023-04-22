using PropertySearchApp.Domain.Abstract;

namespace PropertySearchApp.Domain;

public class LocationDomain : DomainBase
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
