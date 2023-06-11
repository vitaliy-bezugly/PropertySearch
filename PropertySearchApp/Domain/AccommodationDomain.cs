using AutoMapper;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Accommodations;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : DomainBase, IMapFrom<UpdateAccommodationViewModel>,
    IMapFrom<CreateAccommodationViewModel>, IMapFrom<AccommodationEntity>, IMapFrom<AccommodationViewModel>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }
    public string? PhotoUri { get; set; }
    public LocationDomain? Location { get; set; }
    public DateTime CreationTime { get; set; }
    
    void IMapFrom<AccommodationViewModel>.Mapping(Profile profile)
    {
        profile.CreateMap<AccommodationViewModel, AccommodationDomain>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.OwnerId));
    }
}