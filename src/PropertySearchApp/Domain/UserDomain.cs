using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Identities;

namespace PropertySearchApp.Domain;

public class UserDomain : DomainBase, IMapFrom<UserEntity>, IMapFrom<RegistrationFormViewModel>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsLandlord { get; set; }
    public string Information { get; set; } = string.Empty;
}