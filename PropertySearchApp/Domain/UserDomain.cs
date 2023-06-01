using PropertySearchApp.Common.Mappings;
using PropertySearchApp.Domain.Abstract;
using PropertySearchApp.Entities;

namespace PropertySearchApp.Domain;

public class UserDomain : DomainBase, IMapFrom<UserEntity>
{
    public UserDomain() : base()
    {
        Username = Email = Password = Information = String.Empty;
        IsLandlord = false;
    }
    
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLandlord { get; set; }
    public string Information { get; set; }
}