namespace PropertySearchApp.Models;

public class EditUserRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel> Contacts { get; set; }
    public ContactViewModel ContactToAdd { get; set; }
    public string Email { get; set; }

    public EditUserRequest()
    {
        Contacts = new List<ContactViewModel>();
        ContactToAdd = new ContactViewModel { ContactType = string.Empty, Content = string.Empty};
    }
}