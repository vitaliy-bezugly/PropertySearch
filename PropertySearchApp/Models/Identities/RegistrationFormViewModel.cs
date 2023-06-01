using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models.Identities;

public class RegistrationFormViewModel
{
    [Required(ErrorMessage = $"{nameof(Username)} can not be null"), Display(Name = "Username")]
    public string Username { get; set; } = String.Empty;
    [EmailAddress, Display(Name = "Email address")]
    public string Email { get; set; } = String.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
    [Required, Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match"), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = String.Empty;
    [Display(Name = "Landlord")]
    public bool IsLandlord { get; set; }
}