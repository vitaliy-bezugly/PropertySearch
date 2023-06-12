using Microsoft.Build.Framework;

namespace PropertySearch.Api.Models.Identities;

public class ConfirmEmailQuery
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; } = String.Empty;
}