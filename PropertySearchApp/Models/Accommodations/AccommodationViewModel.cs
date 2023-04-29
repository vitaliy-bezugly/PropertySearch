using System.ComponentModel.DataAnnotations;
using PropertySearchApp.Models.Location;

namespace PropertySearchApp.Models.Accommodations;

public class AccommodationViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [DataType(DataType.Url)]
    public string? PhotoUri { get; set; }
    [Required]
    public string OwnerId { get; set; }
    public string? OwnerUsername { get; set; }
    public LocationViewModel Location { get; set; }
    public DateTime CreationTime { get; set; }

    public AccommodationViewModel()
    {
        Title = string.Empty;
        Location = new LocationViewModel();
    }
}