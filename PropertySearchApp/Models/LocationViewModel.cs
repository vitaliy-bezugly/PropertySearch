namespace PropertySearchApp.Models;

public class LocationViewModel
{
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string Address { get; set; }

    public LocationViewModel()
    {
        Country = Region = City = Address = string.Empty;
    }
}
