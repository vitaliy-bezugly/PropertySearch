using System.ComponentModel.DataAnnotations;
using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Models.Location;

public class LocationViewModel : IMapFrom<LocationDomain>
{
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string Country { get; set; } = String.Empty;  
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string Region { get; set; } = String.Empty;
    [Required, DataType(DataType.Text), MinLength(1), MaxLength(128)]
    public string City { get; set; } = String.Empty;
    [Required, DataType(DataType.Text), MinLength(8), MaxLength(256)]
    public string Address { get; set; } = String.Empty;
}
