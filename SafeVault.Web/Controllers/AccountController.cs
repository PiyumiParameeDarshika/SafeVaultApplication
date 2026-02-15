/* SafeVault/src/SafeVault.Web/Controllers/AccountController.cs */
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Models;
using SafeVault.Web.Services;

namespace SafeVault.Web.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _auth;

    public AccountController(AuthService auth) => _auth = auth;

    [HttpGet("/account/login")]
    public IActionResult Login(string? returnUrl = null)
        => View(new LoginModel { ReturnUrl = returnUrl });

    [ValidateAntiForgeryToken]
    [HttpPost("/account/login")]
    public async Task<IActionResult> Login([FromForm] LoginModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        var principal = await _auth.AuthenticateAsync(model.Username!, model.Password!, ct);
        if (principal is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                AllowRefresh = true
            });

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("/account/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet("/account/denied")]
    public IActionResult Denied() => View();
}
