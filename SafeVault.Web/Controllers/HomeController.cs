/* SafeVault/src/SafeVault.Web/Controllers/HomeController.cs */
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index() => View();
}
