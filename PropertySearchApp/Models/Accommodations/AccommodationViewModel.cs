using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Location;
using PropertySearchApp.Models.Pictures;

namespace PropertySearchApp.Models.Accommodations;

public class AccommodationViewModel : IMapFrom<AccommodationDomain>
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    public List<PictureWithoutContentViewModel> Pictures { get; set; } = new();
    public Guid OwnerId { get; set; }
    public string? OwnerUsername { get; set; }
    public LocationViewModel? Location { get; set; } = null!;
    public DateTime CreationTime { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccommodationDomain, AccommodationViewModel>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));
    }
}