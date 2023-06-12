using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;
using PropertySearch.Api.Models.Contacts;

namespace PropertySearch.Api.Models.Identities;

public class EditUserFieldsRequest : IMapFrom<UserDomain>
{
    public EditUserFieldsRequest()
    {
        UserName = string.Empty;
        Information = string.Empty;
        PasswordToCompare = string.Empty;
        Contacts = new List<ContactViewModel>();
    }
    public EditUserFieldsRequest(EditUserFieldsRequest other)
    {
        UserName = other.UserName;
        Information = other.Information;
        Contacts = other.Contacts.Select(x => new ContactViewModel(x)).ToList();
        PasswordToCompare = other.PasswordToCompare;
    }
    
    public string UserName { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel> Contacts { get; set; }
    public string PasswordToCompare { get; set; }
}