using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

// Add profile data for application users by adding properties to the UserEntity class
public class UserEntity : IdentityUser<Guid>, IEntity
{
    [Column("Landlord")]
    public bool IsLandlord { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public ICollection<AccommodationEntity>? Accommodations { get; set; }
}