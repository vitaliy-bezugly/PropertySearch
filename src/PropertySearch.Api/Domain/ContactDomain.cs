using PropertySearch.Api.Common.Mappings;
using PropertySearch.Api.Domain.Abstract;
using PropertySearch.Api.Entities;
using PropertySearch.Api.Models.Contacts;

namespace PropertySearch.Api.Domain;

public class ContactDomain : DomainBase, IMapFrom<ContactEntity>, IMapFrom<ContactViewModel>
{
    public string ContactType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
