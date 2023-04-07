namespace PropertySearchApp.Models;

public class EditUserRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel> Contacts { get; set; }
    public string Email { get; set; }

    public EditUserRequest()
    {
        Contacts = new List<ContactViewModel>();
    }
}