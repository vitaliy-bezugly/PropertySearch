using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace PropertySearchApp.Models;

public class AccommodationViewModel
{
    public Guid Id { get; set; }
    public string? PhotoUri { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string OwnerUsername { get; set; }
    [Required]
    public string OwnerId { get; set; }
}