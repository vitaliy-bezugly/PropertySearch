using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Entities.Abstract;

public abstract class BaseEntity
{
    [Required, Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationTime { get; set; } = DateTime.Now;
}