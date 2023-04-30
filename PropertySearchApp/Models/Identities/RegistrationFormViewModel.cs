using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models.Identities;

public class RegistrationFormViewModel
{
    [Required(ErrorMessage = $"{nameof(Username)} can not be null"), Display(Name = "Username")]
    public string Username { get; set; }
    [EmailAddress, Display(Name = "Email address")]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match"), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
    [Display(Name = "Landlord")]
    public bool IsLandlord { get; set; }
}