/* SafeVault/src/SafeVault.Web/Security/IInputSanitizer.cs */
namespace SafeVault.Web.Security;

public interface IInputSanitizer
{
    string? SanitizeUsername(string? input);
    string? SanitizeEmail(string? input);
}
