namespace PropertySearchApp.Models;

public class EditUserRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}