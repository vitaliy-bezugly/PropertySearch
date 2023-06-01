using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models.Contacts;

public class CreateContactViewModel
{
    [Required]
    public string ContactType { get; set; } = String.Empty;
    [Required]
    public string Content { get; set; } = String.Empty;
}
