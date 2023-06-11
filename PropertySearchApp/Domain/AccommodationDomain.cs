using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Mappings;
using PropertySearchApp.Models.Accommodations;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : DomainBase, IMapFrom<CreateAccommodationViewModel>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }
    public string? PhotoUri { get; set; }
    public LocationDomain? Location { get; set; }
    public DateTime CreationTime { get; set; }
}