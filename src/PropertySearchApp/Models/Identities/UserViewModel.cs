using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Models.Identities;

public class UserViewModel : IMapFrom<UserDomain>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Information { get; set; } = String.Empty;
    public List<ContactViewModel>? Contacts { get; set; } = new();
}
