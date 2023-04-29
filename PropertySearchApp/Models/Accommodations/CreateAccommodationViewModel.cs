using System.ComponentModel.DataAnnotations;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Models.Accommodations;

public class CreateAccommodationViewModel
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    public string? PhotoUri { get; set; }
    [Required]
    public LocationViewModel Location { get; set; }

    public CreateAccommodationViewModel()
    {
        Price = 0;
        Title = string.Empty;
        Description = null;
        Location = new LocationViewModel();
    }
}
