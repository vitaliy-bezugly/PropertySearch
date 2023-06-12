using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Entities.Abstract;

namespace PropertySearch.Api.Entities;

[Table("Contacts")]
public class ContactEntity : EntityBase, IMapFrom<ContactDomain>
{
    [Required, Column(TypeName = "nvarchar(64)")]
    public string ContactType { get; set; } = string.Empty;
    [Required, Column(TypeName = "nvarchar(128)")]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
