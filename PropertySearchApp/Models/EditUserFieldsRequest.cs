namespace PropertySearchApp.Models;

public class EditUserFieldsRequest
{
    public string UserName { get; set; }
    public string Information { get; set; }
    public List<ContactViewModel> Contacts { get; set; }
    public ContactViewModel ContactToAdd { get; set; }
    public string PasswordToCompare { get; set; }
    public EditUserFieldsRequest()
    {
        Contacts = new List<ContactViewModel>();
        ContactToAdd = new ContactViewModel { ContactType = string.Empty, Content = string.Empty};
    }
    public EditUserFieldsRequest(EditUserFieldsRequest other)
    {
        UserName = other.UserName;
        Information = other.Information;
        Contacts = other.Contacts.Select(x => new ContactViewModel(x)).ToList();
        ContactToAdd = new ContactViewModel(other.ContactToAdd);
        PasswordToCompare = other.PasswordToCompare;
    }
}