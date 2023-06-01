using System.ComponentModel.DataAnnotations;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Models.Accommodations;

public class UpdateAccommodationViewModel : IMapFrom<AccommodationDomain>
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    public List<IFormFile>? Pictures { get; set; } = null!;
    public LocationViewModel Location { get; set; }

    public UpdateAccommodationViewModel()
    {
        Price = 0;
        Title = string.Empty;
        Description = null;
        Location = new LocationViewModel();
    }
}
