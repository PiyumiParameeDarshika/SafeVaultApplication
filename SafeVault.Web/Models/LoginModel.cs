/* SafeVault/src/SafeVault.Web/Models/LoginModel.cs */
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Models;

public class LoginModel
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string? Username { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
