/* SafeVault/src/SafeVault.Web/Models/UserInputModel.cs */
using System.ComponentModel.DataAnnotations;
using SafeVault.Web.Validation;

namespace SafeVault.Web.Models;

public class UserInputModel
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [Username] // custom validation attribute
    public string? Username { get; set; }

    [Required]
    [StringLength(320)]
    [EmailAddress]
    [NoControlChars] // blocks CR/LF injection etc.
    public string? Email { get; set; }
}
