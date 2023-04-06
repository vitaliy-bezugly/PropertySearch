using PropertySearchApp.Domain.Abstract;

namespace PropertySearchApp.Domain;

public class UserDomain : BaseDomain
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLandlord { get; set; }
    public string Information { get; set; }
    public List<ContactDomain> Contacts { get; set; }
    public UserDomain() : base()
    {
        Username = String.Empty;
        Email = String.Empty;  
        Password = String.Empty;
        IsLandlord = false;
        Information = String.Empty;
        Contacts = new List<ContactDomain>();
    }
}