/* SafeVault/src/SafeVault.Web/Services/AuthService.cs */
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using SafeVault.Data;
using SafeVault.Web.Security;

namespace SafeVault.Web.Services;

public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public AuthService(IUserRepository users, IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<ClaimsPrincipal?> AuthenticateAsync(string username, string password, CancellationToken ct = default)
    {
        username = (username ?? string.Empty).Trim();

        var user = await _users.GetByUsernameAsync(username, ct);
        if (user is null) return null;

        if (!_hasher.Verify(password, user.PasswordHash))
            return null;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}
