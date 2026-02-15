/* SafeVault/src/SafeVault.Web/Validation/UsernameAttribute.cs */
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SafeVault.Web.Validation;

// Allow only safe characters; reject anything that could be HTML/JS/SQL glue.
public sealed class UsernameAttribute : ValidationAttribute
{
    // 3–30 chars: letters, digits, underscore, dot, hyphen
    private static readonly Regex Rx = new(@"^[A-Za-z0-9_.-]{3,30}$", RegexOptions.Compiled);

    public override bool IsValid(object? value)
    {
        var s = value as string;
        if (string.IsNullOrWhiteSpace(s)) return false;
        return Rx.IsMatch(s);
    }

    public override string FormatErrorMessage(string name)
        => $"{name} must be 3–30 characters and contain only letters, numbers, underscore (_), dot (.), or hyphen (-).";
}
