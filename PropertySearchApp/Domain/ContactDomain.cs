using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Domain;

public class ContactDomain : DomainBase, IMapFrom<ContactEntity>, IMapFrom<ContactViewModel>
{
    public ContactDomain() : base()
    {
        ContactType = String.Empty;
        Content = String.Empty;
    }
    
    public string ContactType { get; set; }
    public string Content { get; set; }
}
