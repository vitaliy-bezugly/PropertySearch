using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

[Table("Accommodation")]
public class AccommodationEntity : BaseEntity
{
    [Required]
    public string Title { get; set; }
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    public int Price { get; set; }
    [ForeignKey(nameof(User))] 
    public string UserId { get; set; }
    public UserEntity? User { get; set; }
}