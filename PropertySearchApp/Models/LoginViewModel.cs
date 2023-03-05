using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models;

public class LoginViewModel
{
    [EmailAddress, Required, Display(Name = "Email address")]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}