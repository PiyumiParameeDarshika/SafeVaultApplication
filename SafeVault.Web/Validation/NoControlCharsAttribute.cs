/* SafeVault/src/SafeVault.Web/Validation/NoControlCharsAttribute.cs */
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Validation;

// Prevent header/log injection and weird parser issues by rejecting ASCII control chars.
public sealed class NoControlCharsAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var s = value as string;
        if (s is null) return true;

        foreach (var ch in s)
        {
            if (char.IsControl(ch)) return false; // includes \r, \n, \t, etc.
        }
        return true;
    }

    public override string FormatErrorMessage(string name)
        => $"{name} contains invalid control characters.";
}
