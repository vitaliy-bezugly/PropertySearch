using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertySearch.Api.Entities.Abstract;

public abstract class EntityBase
{
    [Required, Key]
    public Guid Id { get; set; }
    [Required, Column(TypeName = "datetime2")]
    public DateTime CreationTime { get; set; } = DateTime.Now;   
}
