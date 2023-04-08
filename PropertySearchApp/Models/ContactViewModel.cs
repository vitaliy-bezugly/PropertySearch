namespace PropertySearchApp.Models;

public class ContactViewModel
{
    public string ContactType { get; set; }
    public string Content { get; set; }

    public ContactViewModel()
    {
        ContactType = string.Empty;
        Content = string.Empty;
    }
}
