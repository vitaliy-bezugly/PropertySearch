using PropertySearchApp.Models.Contacts;

namespace PropertySearchApp.Models.Identities;

public class UserDetailsViewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel>? Contacts { get; set; }

    public UserDetailsViewModel()
    {
        Contacts = new List<ContactViewModel>();
    }
}
