using Microsoft.Build.Framework;
using PropertySearchApp.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertySearchApp.Entities;

[Table("Contacts")]
public class ContactEntity : EntityBase
{
    [Required, Column(TypeName = "nvarchar(64)")]
    public string ContactType { get; set; }
    [Required, Column(TypeName = "nvarchar(128)")]
    public string Content { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
