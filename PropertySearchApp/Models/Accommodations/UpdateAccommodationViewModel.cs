using System.ComponentModel.DataAnnotations;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Models.Accommodations;

public class UpdateAccommodationViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [DataType(DataType.Url)]
    public string? PhotoUri { get; set; }
    public LocationViewModel Location { get; set; }

    public UpdateAccommodationViewModel()
    {
        Price = 0;
        Title = string.Empty;
        Description = null;
        Location = new LocationViewModel();
    }
}
