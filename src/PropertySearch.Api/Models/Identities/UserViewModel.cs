using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Models.Contacts;

namespace PropertySearch.Api.Models.Identities;

public class UserViewModel : IMapFrom<UserDomain>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Information { get; set; } = String.Empty;
    public List<ContactViewModel>? Contacts { get; set; } = new();
}
