using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

[Table("Accommodations")]
public class AccommodationEntity : EntityBase, IMapFrom<AccommodationDomain>
{
    [Required, Column(TypeName = "nvarchar(256)")]
    public string Title { get; set; } = string.Empty;
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [DataType(DataType.Url)]
    public string? PhotoUri { get; set; }

    [ForeignKey(nameof(User))] 
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    [ForeignKey(nameof(Location))]
    public Guid LocationId { get; set; }
    public LocationEntity? Location { get; set; }
}