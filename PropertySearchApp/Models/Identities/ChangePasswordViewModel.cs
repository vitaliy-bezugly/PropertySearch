using System.ComponentModel.DataAnnotations;

namespace PropertySearchApp.Models.Identities;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Current password")]
    public string CurrentPassword { get; set; }
    [Required, DataType(DataType.Password), Display(Name = "New password")]
    public string NewPassword { get; set; }
    [Required, DataType(DataType.Password), Display(Name = "Confirm new password"), Compare(nameof(NewPassword), ErrorMessage = "New password and confirmation password do not match")]
    public string ConfirmPassword { get; set; }
}
