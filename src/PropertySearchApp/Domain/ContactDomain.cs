using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;
using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Domain;

public class ContactDomain : DomainBase, IMapFrom<ContactEntity>, IMapFrom<ContactViewModel>
{
    public string ContactType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
