using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities.Abstract;

namespace PropertySearch.Api.Entities;

[Table("Users")]
public class UserEntity : IdentityUser<Guid>, IMapFrom<UserDomain>
{
    [Column(TypeName = "nvarchar(max)")]
    public string? Information { get; set; }
    [Column(name: "Landlord", TypeName = "bit")]
    public bool IsLandlord { get; set; }
    [Required, Column(TypeName = "datetime2")]
    public DateTime CreationTime { get; set; } = DateTime.Now;

    public ICollection<ContactEntity>? Contacts { get; set; }
    public ICollection<AccommodationEntity>? Accommodations { get; set; }
}