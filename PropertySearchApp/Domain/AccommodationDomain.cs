using AutoMapper;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Accommodations;

namespace PropertySearchApp.Domain;

public class AccommodationDomain : DomainBase, IMapFrom<AccommodationEntity>, IMapFrom<AccommodationViewModel>
{
    public AccommodationDomain() : base()
    {
        Title = string.Empty;
        Description = string.Empty;
        Price = 0;
        UserId = Guid.Empty;
        Pictures = new List<PictureDomain>();
    }
    public AccommodationDomain(Guid id, string title, string? description, int price, List<PictureDomain> pictures, Guid userId, LocationDomain location) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        Pictures = pictures;
        UserId = userId;
        Location = location;
    }
    
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public Guid UserId { get; set; }
    public List<PictureDomain> Pictures { get; set; }
    public LocationDomain? Location { get; set; }
    public DateTime CreationTime { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccommodationViewModel, AccommodationDomain>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.OwnerId));
    }
}