using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models;

public class UpdateAccommodationViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? PhotoUri { get; set; }
    public LocationViewModel? Location { get; set; }

    public UpdateAccommodationViewModel()
    {
        Price = 0;
        Title = Description = string.Empty;
        Location = null;
    }
}
