using System.ComponentModel.DataAnnotations;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Models.Accommodations;

public class UpdateAccommodationViewModel : IMapFrom<AccommodationDomain>
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [DataType(DataType.Url)]
    public string? PhotoUri { get; set; }
    public LocationViewModel Location { get; set; } = new();
}
