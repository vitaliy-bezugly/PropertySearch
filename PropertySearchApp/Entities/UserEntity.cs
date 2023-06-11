using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

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