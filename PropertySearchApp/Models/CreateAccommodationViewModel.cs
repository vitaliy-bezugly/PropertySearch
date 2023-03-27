using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models;

public class CreateAccommodationViewModel
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string? PhotoUri { get; set; }
}
