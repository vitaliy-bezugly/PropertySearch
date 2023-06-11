using AutoMapper;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Accommodations;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : DomainBase, IMapFrom<AccommodationEntity>, IMapFrom<AccommodationViewModel>
{
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }
    public List<PictureDomain> Pictures { get; set; } = new();
    public LocationDomain? Location { get; set; }
    public DateTime CreationTime { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccommodationViewModel, AccommodationDomain>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.OwnerId));
    }
}