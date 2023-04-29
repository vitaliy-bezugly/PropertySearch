using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models.Location;

public class LocationViewModel
{
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string Country { get; set; }
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string Region { get; set; }
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string City { get; set; }
    [Required, DataType(DataType.Text), MinLength(8), MaxLength(256)]
    public string Address { get; set; }

    public LocationViewModel()
    {
        Country = Region = City = Address = string.Empty;
    }
}
