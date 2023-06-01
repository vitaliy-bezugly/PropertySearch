using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Domain;

public class PictureDomain : DomainBase, IMapFrom<PictureEntity>
{
    public PictureDomain() : base()
    {
        PictureSource = Array.Empty<byte>();
    }
    
    public byte[] PictureSource { get; set; }
}
