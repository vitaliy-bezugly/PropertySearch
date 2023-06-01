using Microsoft.Build.Framework;

namespace PropertySearchApp.Models.Identities;

public class ConfirmEmailViewModel
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; } = String.Empty;
}