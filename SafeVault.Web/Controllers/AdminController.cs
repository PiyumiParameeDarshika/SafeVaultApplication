/* SafeVault/src/SafeVault.Web/Controllers/AdminController.cs */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Web.Controllers;

[Authorize(Policy = "AdminOnly")]
public class AdminController : Controller
{
    [HttpGet("/admin")]
    public IActionResult Dashboard() => View();
}
