using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PropertySearchApp.Entities;

// Add profile data for application users by adding properties to the UserEntity class
public class UserEntity : IdentityUser
{
    [Column("Landlord")]
    public bool IsLandlord { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
}