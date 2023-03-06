using AutoMapper;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Models;

namespace PropertySearchApp.Extensions;

public class ApplicationMapperExtension : Profile
{
    public ApplicationMapperExtension()
    {
        CreateMap<UserDomain, UserEntity>();
        CreateMap<RegistrationFormViewModel, UserDomain>();
    }
}
