using AutoMapper;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Accommodations;
using PropertySearchApp.Models.Contacts;
using PropertySearchApp.Models.Identities;
using PropertySearchApp.Models.Location;

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
        CreateMap<AccommodationDomain, UpdateAccommodationViewModel>();

        CreateMap<UserEntity, UserDomain>().ReverseMap();
        CreateMap<UserDomain, UserDetailsViewModel>();
        CreateMap<UserDomain, EditUserFieldsRequest>();

        CreateMap<ContactEntity, ContactDomain>().ReverseMap();
        CreateMap<ContactDomain, ContactViewModel>().ReverseMap();

        CreateMap<LocationEntity, LocationDomain>().ReverseMap();
        CreateMap<LocationDomain, LocationViewModel>().ReverseMap();

        CreateMap<RegistrationFormViewModel, UserDomain>();
    }
}
