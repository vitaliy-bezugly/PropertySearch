using System.ComponentModel.DataAnnotations;

namespace PropertySearch.Api.Models.Identities;

public class LoginViewModel
{
    [Required, Display(Name = "Username")]
    public string Username { get; set; } = String.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
}