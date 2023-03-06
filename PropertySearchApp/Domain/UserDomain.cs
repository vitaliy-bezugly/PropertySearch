namespace PropertySearchApp.Domain;

public class UserDomain
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLandlord { get; set; }

    public UserDomain()
    {
        Username = String.Empty;
        Email = String.Empty;  
        Password = String.Empty;
        IsLandlord = false;
    }
    public UserDomain(string username, string email, string password, bool isLandlord)
    {
        Username = username;
        Email = email;
        Password = password;
        IsLandlord = isLandlord;
    }
}