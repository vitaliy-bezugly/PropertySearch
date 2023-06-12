using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities.Abstract;

namespace PropertySearch.Api.Entities
{
    [Table("Locations")]
    public class LocationEntity : EntityBase, IMapFrom<LocationDomain>
    {
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Country { get; set; }  = string.Empty;
        [Required, Column(TypeName = "nvarchar(128)")]
        public string Region { get; set; }  = string.Empty;
        [Required, Column(TypeName = "nvarchar(128)")]
        public string City { get; set; }  = string.Empty;
        [Required, Column(TypeName = "nvarchar(256)")]
        public string Address { get; set; }  = string.Empty;

        public AccommodationEntity? Accommodation { get; set; }
    }
}
