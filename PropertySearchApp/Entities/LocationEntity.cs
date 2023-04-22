using PropertySearchApp.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertySearchApp.Entities
{
    [Table("Locations")]
    public class LocationEntity : EntityBase
    {
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Country { get; set; }
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Region { get; set; }
        [Required, Column(TypeName = "nvarchar(128)")]
        public string City { get; set; }
        [Required, Column(TypeName = "nvarchar(256)")]
        public string Address { get; set; }

        public AccommodationEntity? Accommodation { get; set; }
    }
}
