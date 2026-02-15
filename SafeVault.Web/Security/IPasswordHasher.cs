/* SafeVault/src/SafeVault.Web/Security/IPasswordHasher.cs */
namespace SafeVault.Web.Security;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
