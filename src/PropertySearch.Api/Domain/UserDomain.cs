using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain.Abstract;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Models.Identities;

namespace PropertySearch.Api.Domain;

public class UserDomain : DomainBase, IMapFrom<UserEntity>, IMapFrom<RegistrationFormViewModel>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsLandlord { get; set; }
    public string Information { get; set; } = string.Empty;
}