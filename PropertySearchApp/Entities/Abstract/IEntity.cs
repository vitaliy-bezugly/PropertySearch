using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Entities.Abstract;

public interface IEntity
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
}