namespace PropertySearchApp.Domain;

public class UserDomain
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public bool IsLandlord { get; private set; }

    public UserDomain(string username, string email, string password, bool isLandlord)
    {
        Username = username;
        Email = email;
        Password = password;
        IsLandlord = isLandlord;
    }
}