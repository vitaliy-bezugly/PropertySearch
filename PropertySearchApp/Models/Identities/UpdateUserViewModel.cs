using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Models.Identities;

public class UpdateUserViewModel : IMapFrom<UserDomain>
{
    public string Username { get; set; } = String.Empty;
    public string Information { get; set; } = String.Empty;
    public List<ContactViewModel> Contacts { get; set; } = new();
    public string PasswordToCompare { get; set; } = String.Empty;
}