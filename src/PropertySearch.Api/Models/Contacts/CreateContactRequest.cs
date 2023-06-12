using System.ComponentModel.DataAnnotations;

namespace PropertySearch.Api.Models.Contacts;

public class CreateContactRequest
{
    [Required]
    public string ContactType { get; set; } = String.Empty;
    [Required]
    public string Content { get; set; } = String.Empty;
}
