using PropertySearchApp.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Entities
{
    [Table("Locations")]
    public class LocationEntity : EntityBase, IMapFrom<LocationDomain>
    {
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Country { get; set; } = String.Empty;
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Region { get; set; } = String.Empty;
        [Required, Column(TypeName = "nvarchar(128)")]
        public string City { get; set; } = String.Empty;
        [Required, Column(TypeName = "nvarchar(256)")]
        public string Address { get; set; } = String.Empty;

        public AccommodationEntity? Accommodation { get; set; } = null!;
    }
}
