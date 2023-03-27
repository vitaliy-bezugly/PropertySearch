using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

[Table("Accommodation")]
public class AccommodationEntity : IEntity
{
    [Required, Key]
    public Guid Id { get; set; } = Guid.Empty;
    [Required]
    public string Title { get; set; }
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? PhotoUri { get; set; }
    [ForeignKey(nameof(User))] 
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
}