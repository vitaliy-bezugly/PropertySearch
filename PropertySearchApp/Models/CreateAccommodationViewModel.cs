using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models;

public class CreateAccommodationViewModel
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string? PhotoUri { get; set; }
    [Required]
    public LocationViewModel Location { get; set; }

    public CreateAccommodationViewModel()
    {
        Price = 0;
        Title = Description = string.Empty;
        Location = new LocationViewModel();
    }
}
