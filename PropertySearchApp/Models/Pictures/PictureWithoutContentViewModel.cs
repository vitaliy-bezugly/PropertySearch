using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;

namespace PropertySearchApp.Models.Pictures;

public class PictureWithoutContentViewModel : IMapFrom<PictureDomain>
{
    public Guid Id { get; set; }
}