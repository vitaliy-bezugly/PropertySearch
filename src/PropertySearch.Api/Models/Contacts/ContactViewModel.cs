using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain;

namespace PropertySearch.Api.Models.Contacts;

public class ContactViewModel : IMapFrom<ContactDomain>
{
    public Guid Id { get; set; }
    public string ContactType { get; set; }
    public string Content { get; set; }

    public ContactViewModel()
    {
        Id = Guid.Empty;
        ContactType = String.Empty;
        Content = String.Empty;
    }
    
    public ContactViewModel(ContactViewModel other)
    {
        Id = other.Id;
        ContactType = other.ContactType;
        Content = other.Content;
    }
}
