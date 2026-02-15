/* SafeVault/src/SafeVault.Web/Security/InputSanitizer.cs */
using System.Text;
using System.Text.RegularExpressions;

namespace SafeVault.Web.Security;

public sealed class InputSanitizer : IInputSanitizer
{
    // Remove common HTML tag payload characters. (Validation will still enforce strict patterns.)
    private static readonly Regex DangerousChars = new(@"[<>""'`;(){}\[\]\\]", RegexOptions.Compiled);

    public string? SanitizeUsername(string? input)
    {
        if (input is null) return null;
        var s = input.Trim();

        // Remove angle brackets, quotes, semicolons etc.
        s = DangerousChars.Replace(s, string.Empty);

        // Normalize to avoid weird unicode confusables in usernames (simple baseline).
        s = s.Normalize(NormalizationForm.FormKC);

        return s;
    }

    public string? SanitizeEmail(string? input)
    {
        if (input is null) return null;
        var s = input.Trim();

        // Remove obvious HTML/JS glue characters; keep email structure intact.
        s = DangerousChars.Replace(s, string.Empty);

        // Emails are case-insensitive in domain part; keep as-is or optionally lower-case whole.
        s = s.Normalize(NormalizationForm.FormKC);

        return s;
    }
}
