using System.ComponentModel.DataAnnotations.Schema;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities.Abstract;

namespace PropertySearchApp.Entities;

[Table("Pictures")]
public class PictureEntity : EntityBase, IMapFrom<PictureDomain>
{
    public byte[] PictureSource { get; set; } = Array.Empty<byte>();
}