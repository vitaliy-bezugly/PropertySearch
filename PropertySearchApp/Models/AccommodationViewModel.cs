using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace PropertySearchApp.Models;

public class AccommodationViewModel
{
    public Image? MainPhoto { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    [Required, EmailAddress]
    public string LandlordEmail { get; set; }
}