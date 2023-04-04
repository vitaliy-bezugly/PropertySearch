using AutoMapper;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Models;

namespace PropertySearchApp.Extensions;

public class ApplicationMapperExtension : Profile
{
    public ApplicationMapperExtension()
    {
        CreateMap<AccommodationViewModel, AccommodationDomain>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.Parse(src.OwnerId)));
        CreateMap<AccommodationDomain, AccommodationViewModel>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId.ToString()));
        CreateMap<AccommodationDomain, AccommodationEntity>().ReverseMap();
        
        CreateMap<UserEntity, UserDomain>();
        CreateMap<UserDetailsViewModel, UserDomain>().ReverseMap();
        CreateMap<AccommodationDomain, UpdateAccommodationViewModel>();

        CreateMap<UserDomain, UserEntity>();
        CreateMap<RegistrationFormViewModel, UserDomain>();

    }
}
