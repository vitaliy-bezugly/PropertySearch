using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain;
using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Models.Identities;

public class EditUserFieldsRequest : IMapFrom<UserDomain>
{
    public string UserName { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel> Contacts { get; set; }
    public string PasswordToCompare { get; set; }
    public EditUserFieldsRequest()
    {
        Contacts = new List<ContactViewModel>();
    }
    public EditUserFieldsRequest(EditUserFieldsRequest other)
    {
        UserName = other.UserName;
        Information = other.Information;
        Contacts = other.Contacts.Select(x => new ContactViewModel(x)).ToList();
        PasswordToCompare = other.PasswordToCompare;
    }
}