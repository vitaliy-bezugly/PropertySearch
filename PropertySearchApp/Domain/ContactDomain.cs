using PropertySearchApp.Domain.Abstract;

namespace PropertySearchApp.Domain;

public class ContactDomain : BaseDomain
{
    public string ContactType { get; set; }
    public string Content { get; set; }
}
